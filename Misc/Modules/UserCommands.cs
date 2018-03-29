using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Misc.Modules.CasesSystem;
using Discord_BOT.Misc.Modules.ColorsManagment;

namespace Discord_BOT.Misc.Modules
{
    public class UserCommands : ModuleBase<SocketCommandContext>
    {
        [Command("opisz")]
        public async Task DescribeUser([Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            var target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            string vip = account.Vip ? ":regional_indicator_v: :regional_indicator_i: :regional_indicator_p:" : "";

            var embed = new EmbedBuilder();
            embed.WithTitle($"{vip} {target.Username}");
            embed.WithColor(Colors.List[account.ActuallyColor].RightColor);
            embed.AddInlineField("Poziom", account.Level);
            embed.AddInlineField("Exp", account.Xp);
            embed.AddField("Dudy", account.Cash);

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("kitvip")]
        public async Task GiveKitVip()
        {
            var account = UserAccounts.GetAccount(Context.User);
            var requiredTime = new TimeSpan(0, 0, Config.Bot.KitVipTime);

            if (account.Vip == false)
            {
                await Context.Channel.SendMessageAsync(":warning: Komenda dostepna tylko dla vipow!");
                return;
            }

            if (Checker.WasRequiredTimeElapsed(account.LastKitVipUse, requiredTime))
            {
                for (int i = 0; i < 5; i++)
                {
                    account.Inventory.Add(CasesSystem.Cases.List.Find((x => x.ID == 100)));
                    account.Inventory.Add(new Key(Item.QUALITY.BRONZE, 3));
                }

                await Context.Channel.SendMessageAsync(
                    $":regional_indicator_v: :regional_indicator_i: :regional_indicator_p:" +
                    $"Dostales **5 :lock:Brazowych Skrzynek** oraz **5 :key:Brazowych Kluczy +3!**");

                account.LastKitVipUse = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                var waitingTime = account.LastKitVipUse + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($":warning: Hola hola, musisz poczekac jeszcze :clock1:" +
                $"`{waitingTime.Hours}h:{waitingTime.Minutes}m` zanim bedziesz mogl pobrac kolejne swiadczenie!.");
            }
        }

        [Command("500+")]
        public async Task GiveBigDotation()
        {
            var account = UserAccounts.GetAccount(Context.User);
            var requiredTime = new TimeSpan(0, 0, Config.Bot.BigDotationTime);

            if (Checker.WasRequiredTimeElapsed(account.LastBigDotationUse, requiredTime))
            {
                account.Cash += Config.Bot.BigDotationAmount;
                await Context.Channel.SendMessageAsync($":moneybag: Gratuluje dziecka! Dostales " +
                $":small_orange_diamond:**{Config.Bot.BigDotationAmount}** Dudow.");

                account.LastBigDotationUse = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                var waitingTime = account.LastBigDotationUse + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($":warning: Hola hola, musisz poczekac jeszcze :clock1:" +
                    $"`{waitingTime.Hours}h:{waitingTime.Minutes}m` " +
                    $"zanim bedziesz mogl znowu uzyc zestawu VIP!.");
            }
        }

        [Command("kierowniku")]
        public async Task GiveLittleDotation()
        {
            var account = UserAccounts.GetAccount(Context.User);

            var requiredTime = new TimeSpan(0, 0, Config.Bot.LittleDotationTime);
            if (Checker.WasRequiredTimeElapsed(account.LastLittleDotationUse, requiredTime))
            {
                account.Cash += Config.Bot.LittleDotationAmount;
                await Context.Channel.SendMessageAsync($":small_orange_diamond:**" +
                $"{Config.Bot.LittleDotationAmount}** Dudow dla ksieciunia!");

                account.LastLittleDotationUse = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                var waitingTime = account.LastLittleDotationUse + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($":warning: Ejze, Menelaosie. Poczekaj ze :clock1: " +
                    $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s` na kolejny datek!");
            }
        }

        [Command("przelew")]
        public async Task GiveCashOtherUser(int amount, [Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            var target = mentionedUser ?? Context.User;

            if (target is null)
                await Context.Channel.SendMessageAsync(":warning: Nie ma takiego robaka.");
            if (target == Context.User)
                await Context.Channel.SendMessageAsync(":warning: Glupi czy glupi?");

            var sender = UserAccounts.GetAccount(Context.User);
            var recipient = UserAccounts.GetAccount(target);
            await DoTransferIfFullfillConditions(sender, recipient, amount);
        }

        private async Task DoTransferIfFullfillConditions(UserAccount sender, UserAccount recipient, int amount)
        {
            var requiredTime = new TimeSpan(Config.Bot.TransferTime);

            if (Checker.WasRequiredTimeElapsed(sender.LastTransfer, requiredTime) == false)
            {
                var waitingTime = sender.LastTransfer + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($":warning: Kolejny przelew dostepny za :clock1: " +
                    $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s`.");
                return;
            }

            if (sender.Cash < amount)
            {
                await Context.Channel.SendMessageAsync($":warning: Tfu, biedaku robaku. Nie masz tyle hajsu.");
                return;
            }

            if (amount < Config.Bot.MinimalTransfer)
            {
                await Context.Channel.SendMessageAsync($":warning: Minimalna kwota przelewu to :small_orange_diamond: " +
                $"{Config.Bot.MinimalTransfer}** Dudow.");
                return;
            }

            await DoTransfer(sender, recipient, amount);
        }

        private async Task DoTransfer(UserAccount sender, UserAccount recipient, int amount)
        {
            sender.Cash -= amount;
            sender.Xp += amount / 10;

            double tax = amount * 0.15;
            recipient.Cash += amount - (int)tax;

            UserAccounts.GetAccount(Config.Bot.Id).Cash += (int)tax;
            UserAccounts.GetAccount(Config.Bot.Id).Xp++;

            await Context.Channel.SendMessageAsync($":white_check_mark: Przelew udany. Dostales " +
            $":small_blue_diamond:**{amount / 10}** XP. Prowizja: :small_orange_diamond:**{tax}** Dudow.");

            sender.LastTransfer = DateTime.Now;
            UserAccounts.SaveAccounts();
        }

        [Command("ilebrakuje")]
        public async Task HowMuch()
        {
            var account = UserAccounts.GetAccount(Context.User);
            long howMuch = LevelCounter.HowMuchExpNeedForNextLevel(account.Level, account.Xp);
            await Context.Channel.SendMessageAsync($"Do kolejnego poziomu **{account.Level + 1}** " +
            $"brakuje ci :small_blue_diamond:**{howMuch}** expa");
        }

        [Command("ilena")]
        public async Task HowMuch(int level)
        {
            long howMuch = LevelCounter.HowMuchExpFor(level);
            await Context.Channel.SendMessageAsync($"Na poziom **{level}** jest wymagane :small_blue_diamond:**{howMuch}** expa.");
        }

        [Command("mojczas")]
        public async Task ShowMyTime()
        {
            var user = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}, na serwerze spedziles :clock1:" +
            $" `{user.TimeSpendOnDiscord}`");
        }
    }
}
