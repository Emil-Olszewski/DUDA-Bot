using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Modules.ColorsManagment;

namespace Discord_BOT.Misc.Modules.Investment
{
    public class InvestmentUI : ModuleBase<SocketCommandContext>
    {
        [Command("mojelokaty")]
        public async Task ShowUserInvestments()
        {
            var account = UserAccounts.GetAccount(Context.User);
            List<Investment> investments = InvestmentsManager.GetInvestments(account);
            if (investments.Count == 0)
                await Context.Channel.SendMessageAsync("Nie posiadasz zadnych lokat.");
            else
            {
                var embed = new EmbedBuilder();
                embed.WithTitle($"Lokaty {Context.User.Username}");
                embed.WithColor(Colors.AvalibleColors[(int)account.ActuallyColor].RightColor);

                foreach (var i in investments)
                {
                    embed.AddField($"[{i.ID}] Zalozono: `{i.PaymentDate}`", $"" +
                        $"Wyplata: `{i.MoneyToWithdraw}` dudow. Koniec: `{i.PaymentDate+i.InvestmentDuration}`");
                }

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("zalozlokate")]
        public async Task CreateInvestment(int amount, int hours)
        {
            if (amount < Config.Bot.minimalInvestment)
            {
                await Context.Channel.SendMessageAsync($"Minimalna lokata to " +
                    $"**{Config.Bot.minimalInvestment}** dudow");
                return;
            }

            if (hours < Config.Bot.minimalInvestmentTime)
            {
                await Context.Channel.SendMessageAsync($"Minimalny czas lokaty " +
                    $"to **{Config.Bot.minimalInvestmentTime}** godzin");
                return;
            }

            var account = UserAccounts.GetAccount(Context.User);

            Random randomizer = new Random();
            
            int id = GenerateRandomID(amount);
            InvestmentsManager.AddInvestment(new Investment((uint)id, account.ID, DateTime.Now, 
                new TimeSpan(hours, 0, 0), amount));

            account.Cash -= amount;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync("Pomyslnie utworzono lokate");
        }

        private int GenerateRandomID(int amount)
        {
            Random randomizer = new Random();
            return DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Hour +
                DateTime.Now.Minute + amount * 3 + randomizer.Next(10, 100);
        }

        [Command("wyplaclokaty")]
        public async Task DoPossibleWithdraws()
        {
            var account = UserAccounts.GetAccount(Context.User);
            List<Investment> investments = InvestmentsManager.GetInvestments(account);

            if (investments.Count == 0)
                await Context.Channel.SendMessageAsync("Nie posiadasz zadnych lokat.");
            else
            {
                int cash = 0, investmentsDeleted = 0;
                foreach (var i in investments)
                {
                    if(DateTime.Now >= i.PaymentDate + i.InvestmentDuration)
                    {
                        account.Cash += i.MoneyToWithdraw;
                        cash += i.MoneyToWithdraw;
                        InvestmentsManager.DeleteInvestment(i.ID);
                        investmentsDeleted++;
                    }
                }

                if(investmentsDeleted > 0)
                {
                    await Context.Channel.SendMessageAsync($"Wyplacono **{cash}** dudow z " +
                        $"**{investmentsDeleted}** lokat.");

                    UserAccounts.SaveAccounts();
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Zadna z twoich lokat sie nie zakonczyla");
                }
            }
        }
    }
}
