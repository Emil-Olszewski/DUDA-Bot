using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Modules.ColorsManagment;
using Misceleanous = Discord_BOT.Misc.Misc;

namespace Discord_BOT
{
    public class UserCommands : ModuleBase<SocketCommandContext>
    {
        [Command("opisz")]
        public async Task DescribeUser([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            var embed = new EmbedBuilder();
            embed.WithTitle($"{target.Username}");
            embed.WithColor(Colors.AvalibleColors[account.ActuallyColor].RightColor);
            embed.AddInlineField("Poziom spoleczny", account.Level);
            embed.AddInlineField("Punkty statusu spolecznego", account.XP);
            embed.AddField("Dudy", account.Cash);
            embed.AddInlineField("Skrzynek", account.NumberOfCases);
            embed.AddInlineField("Kluczy", account.NumberOfKeys);

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("500+")]
        public async Task Give500Cash()
        {
            var account = UserAccounts.GetAccount(Context.User);
            TimeSpan requiredTime = new TimeSpan(0, 0, Config.Bot.time500);

            if (Misceleanous.WasRequiredTimeElapsed(account.last500Use, requiredTime))
            {
                account.Cash += 500;
                await Context.Channel.SendMessageAsync($"Gratuluje dziecka! Dostales **500** dudow.");

                account.last500Use = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                TimeSpan waitingTime = account.last500Use + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Hola hola, musisz poczekac jeszcze " +
                    $"`{waitingTime.Hours}h:{waitingTime.Minutes}m` " +
                    $"zanim bedziesz mogl pobrac kolejne swiadczenie!.");
            }
        }

        [Command("kierowniku")]
        public async Task Give10Cash()
        {
            var account = UserAccounts.GetAccount(Context.User);

            TimeSpan requiredTime = new TimeSpan(0, 0, Config.Bot.time10);
            if (Misceleanous.WasRequiredTimeElapsed(account.last10Use, requiredTime))
            {
                account.Cash += 10;
                await Context.Channel.SendMessageAsync($"**10** dudow dla ksieciunia!");

                account.last10Use = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                TimeSpan waitingTime = account.last10Use + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Ejze, Menelaosie. Poczekaj ze " +
                    $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s` na kolejny datek!");
            }
        }

        [Command("przelew")]
        public async Task GiveCashOtherUser(int amount, [Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();

            if (target == null)
                await Context.Channel.SendMessageAsync("Nie ma takiego robaka.");
            if (target == Context.User)
                await Context.Channel.SendMessageAsync("Glupi czy glupi?");

            var sender = UserAccounts.GetAccount(Context.User);
            var recipient = UserAccounts.GetAccount(target);

            await DoTransferIfFullfillConditions(sender, recipient, amount);
        }

        private async Task DoTransferIfFullfillConditions(UserAccount sender, UserAccount recipient, int amount)
        {
            TimeSpan requiredTime = new TimeSpan(Config.Bot.timeTransfer);

            if (!Misceleanous.WasRequiredTimeElapsed(sender.lastTransfer, requiredTime))
            {
                TimeSpan waitingTime = sender.lastTransfer + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Kolejny przelew dostepny za " +
                    $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s`.");
                return;
            }

            if (sender.Cash < amount)
            {
                await Context.Channel.SendMessageAsync($"Tfu, biedaku robaku. Nie masz tyle hajsu.");
                return;
            }
            
            if (amount < Config.Bot.minimalTransfer)
            {
                await Context.Channel.SendMessageAsync($"Minimalna kwota przelewu to **{Config.Bot.minimalTransfer}** dudow.");
                return;
            }

            await DoTransfer(sender, recipient, amount);
        }

        private async Task DoTransfer(UserAccount sender, UserAccount recipient, int amount)
        {
            sender.Cash -= amount;
            sender.XP += amount / 10;

            double tax = amount * 0.15;
            recipient.Cash += amount - (int)tax;

            UserAccounts.GetAccount(421676491345100800).Cash += (int)tax;
            UserAccounts.GetAccount(421676491345100800).XP++;

            await Context.Channel.SendMessageAsync($"Przelew udany. Dostales **{amount / 10}** punktow statusu spolecznego. " +
                $"Prowizja: **{tax}** dudow.");

            sender.lastTransfer = DateTime.Now;
            UserAccounts.SaveAccounts();
        }

        [Command("ilebrakuje")]
        public async Task HowMuch()
        {
            var account = UserAccounts.GetAccount(Context.User);
            int howMuch = (int)(2 * Math.Pow(account.Level + 1, 3)) + 50 - account.XP;
            await Context.Channel.SendMessageAsync($"Do kolejnego poziomu **{account.Level+1}** brakuje ci **{howMuch}** expa");
        }

        [Command("ilena")]
        public async Task HowMuch(int level)
        {
            int howMuch = (int)(2 * Math.Pow(level, 3)) + 50;
            await Context.Channel.SendMessageAsync($"Na poziom **{level}** jest wymagane **{howMuch}** punktow spolecznych.");
        }          
    }
}
