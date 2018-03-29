using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Misc.Modules.ColorsManagment;

namespace Discord_BOT.Misc.Modules.Investment
{
    public class InvestmentUi : ModuleBase<SocketCommandContext>
    {
        [Command("mojelokaty")]
        public async Task ShowUserInvestments()
        {
            var account = UserAccounts.GetAccount(Context.User);
            var investments = InvestmentsManager.GetInvestments(account);
            if (investments.Count == 0)
                await Context.Channel.SendMessageAsync(":warning: Nie posiadasz zadnych lokat.");
            else
            {
                var embed = new EmbedBuilder();
                embed.WithTitle($"Lokaty {Context.User.Username}");
                embed.WithColor(Colors.List[account.ActuallyColor].RightColor);

                foreach (var i in investments)
                {
                    embed.AddField($":clock1: Zalozono: `{i.PaymentDate}`", $"" +
                        $"Wyplata: `{i.MoneyToWithdraw}` dudow. Koniec: `{i.PaymentDate + i.InvestmentDuration}`");
                }

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("zalozlokate")]
        public async Task CreateInvestment(int amount, int hours)
        {
            if (amount < Config.Bot.MinimalInvestment)
            {
                await Context.Channel.SendMessageAsync($":warning: Minimalna lokata to " +
                    $"**{Config.Bot.MinimalInvestment}** dudow.");
                return;
            }

            if (hours < Config.Bot.MinimalInvestmentTime)
            {
                await Context.Channel.SendMessageAsync($":warning: Minimalny czas lokaty " +
                    $"to **{Config.Bot.MinimalInvestmentTime}** godzin.");
                return;
            }

            var account = UserAccounts.GetAccount(Context.User);
            if (amount > account.Cash)
            {
                await Context.Channel.SendMessageAsync(":warning: Nie masz tyle dudow!");
                return;
            }

            int id = GenerateRandomId();
            InvestmentsManager.AddInvestment(new Investment((uint)id, account.Id, DateTime.Now,
                new TimeSpan(hours, 0, 0), amount));

            account.Cash -= amount;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync(":inbox_tray: Pomyslnie utworzono lokate.");
        }

        private static int GenerateRandomId()
        {
            var randomizer = new Random();
            return DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Hour +
                DateTime.Now.Minute + randomizer.Next(1, 10000);
        }

        [Command("wyplaclokaty")]
        public async Task DoPossibleWithdraws()
        {
            var account = UserAccounts.GetAccount(Context.User);
            var investments = InvestmentsManager.GetInvestments(account);

            if (investments.Count == 0)
                await Context.Channel.SendMessageAsync(":warning: Nie posiadasz zadnych lokat.");
            else
            {
                int cash = 0, investmentsDeleted = 0;
                foreach (var i in investments)
                {
                    if (DateTime.Now >= i.PaymentDate + i.InvestmentDuration)
                    {
                        account.Cash += i.MoneyToWithdraw;
                        cash += i.MoneyToWithdraw;
                        InvestmentsManager.DeleteInvestment(i.Id);
                        investmentsDeleted++;
                    }
                }

                if (investmentsDeleted > 0)
                {
                    await Context.Channel.SendMessageAsync($":outbox_tray: Wyplacono **{cash}** dudow z " +
                        $"**{investmentsDeleted}** lokat.");

                    UserAccounts.SaveAccounts();
                }
                else
                {
                    await Context.Channel.SendMessageAsync(":warning: Zadna z twoich lokat sie nie zakonczyla.");
                }
            }
        }
    }
}
