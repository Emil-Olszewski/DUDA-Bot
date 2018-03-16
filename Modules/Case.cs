using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Modules
{
    public class Case : ModuleBase<SocketCommandContext>
    {
        const int scale = 200;
        Prize[] prizes =
           {
                new Prize("50 000 DUDOW!", cash: 50000, chance: 199),
                new Prize("10 000 DUDOW!", cash: 10000, chance: 197),
                new Prize("5 000 DUDOW!", cash: 5000, chance: 187),
                new Prize("1 000 DUDOW!", cash: 1000, chance: 167),
                new Prize("800 DUDOW!", cash: 800, chance: 140),
                new Prize("500 Dudow", cash: 500, chance: 110),
                new Prize("300 Dudow", cash: 300, chance: 80),
                new Prize("200 Dudow", cash: 200, chance: 40),
                new Prize("100 Dudow", cash: 5000, chance: 0),
            };

        const int CASE_PRICE = 100;
        const int KEY_PRICE = 700;

        [Command("os")]
        public async Task OpenCaseShortcut()
        {
            await OpenCase();
        }

        [Command("otworzskrzynke")]
        public async Task OpenCase()
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (!CanOpen(account)) return;

            Prize prize = Prize.GetRandomPrize(prizes, scale); 

            await Context.Channel.SendMessageAsync($"Wygrales **{prize.Name}**");
            GivePrizeWinner(account, prize);
        }

        private bool CanOpen(UserAccount account)
        {
            TimeSpan requiredTime = new TimeSpan(0, 0, Int32.Parse(Config.bot.timecase));
            if (!Misc.WasRequiredTimeElapsed(account.lastCaseOpen, requiredTime))
            {
                TimeSpan waitingTime = account.lastCaseOpen + requiredTime - DateTime.Now;
                Context.Channel.SendMessageAsync($"Kolejna skrzynke otworzysz za `{waitingTime.Seconds}s`.");
                return false;
            }

            if (account.NumberOfCases == 0 || account.NumberOfKeys == 0)
            {
                Context.Channel.SendMessageAsync("Nie posiadasz skrzynki lub klucza");
                return false;
            }

            return true;
        }

        private static void GivePrizeWinner(UserAccount account, Prize prize)
        {
            account.Cash += prize.Cash;
            account.NumberOfKeys--;
            account.NumberOfCases--;
            account.CasesProfit += (uint)prize.Cash;
            account.OpenedCases++;
            account.lastCaseOpen = DateTime.Now;
            UserAccounts.SaveAccounts();
        }

        [Command("kupzestaw")]
        public async Task BuySet(uint amount = 1)
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (amount * KEY_PRICE + amount * CASE_PRICE > account.Cash)
            {
                await Context.Channel.SendMessageAsync("Nie masz wystarczajaco duzo dudow.");
                return;
            }

            await BuyKey(amount);
            await BuyCase(amount);
        }

        [Command("kupskrzynke")]
        public async Task BuyCase(uint amount = 1)
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (account.Cash >= CASE_PRICE * amount)
            {
                await Context.Channel.SendMessageAsync($"Dokonano zakupu skrzynki **x{amount}**.");
                account.Cash -= CASE_PRICE * (int)amount;
                account.NumberOfCases += amount;
                UserAccounts.SaveAccounts();
            }
            else
                await Context.Channel.SendMessageAsync("Nie masz wystarczajaco duzo dudow.");
        }

        [Command("kupklucz")]
        public async Task BuyKey(uint amount = 1)
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (account.Cash >= KEY_PRICE * amount)
            {
                await Context.Channel.SendMessageAsync($"Dokonano zakupu klucza **x{amount}**.");
                account.Cash -= KEY_PRICE * (int)amount;
                account.NumberOfKeys += amount;
                UserAccounts.SaveAccounts();
            }
            else
                await Context.Channel.SendMessageAsync("Nie masz wystarczajaco duzo dudow.");
        }

        [Command("skrzynkistaty")]
        public async Task CasesStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"Otworzyles **{account.OpenedCases}** skrzynek i wygrales z nich " +
                $"**{account.CasesProfit}** dudow.");
        }

        [Command("ilemam")]
        public async Task HowMuchIHave()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"Masz **{account.NumberOfCases}** skrzynek i " +
                $"**{account.NumberOfKeys}** kluczy.");
        }
    }
}
