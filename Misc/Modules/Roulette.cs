using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;
using Misceleanous = Discord_BOT.Misc.Misc;

namespace Discord_BOT.Modules
{
    public class Roulette : ModuleBase<SocketCommandContext>
    {
        struct Prize { public int XP; public int Cash; };

        [Command("ruletka all")]
        public async Task PlayRouletteAllCash()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await PlayRoulette(account.Cash);
        }

        [Command("ruletka half")]
        public async Task PlayRouletteHalfCash()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await PlayRoulette(account.Cash / 2);
        }

        [Command("ruletka")]
        public async Task PlayRoulette(int placedCash)
        {
            var account = UserAccounts.GetAccount(Context.User);

            TimeSpan requiredTime = new TimeSpan(0, 0, Config.Bot.timeRoulette);
            if (!Misceleanous.WasRequiredTimeElapsed(account.lastRoulette, requiredTime))
            {
                TimeSpan waitingTime = account.lastRoulette + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Kolejna gra dostepna za " +
                  $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s`.");
                return;
            }

            if (placedCash > account.Cash)
            {
                await Context.Channel.SendMessageAsync($"Kanciarza chcesz kantowac?");
                return;
            }

            if (placedCash < 20)
            {
                await Context.Channel.SendMessageAsync($"Minimalna kwota zakladu to 20 dudow.");
                return;
            }

            double chance = CountChance(account.Cash, placedCash);
            Prize prize = DoDraw(placedCash, chance);

            if(prize.Cash > 0)
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} **WYGRALES!** Miales na to szanse **{(int)chance}%** i zarobiles **{prize.Cash}** dudow. Dodatkowo dostajesz {prize.XP} punktow statusu spolecznego.");
            else
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} **PRZEGRALES.** Szansa na wygrana wynosila **{(int)chance}%** i straciles **{prize.Cash}** dudow. Udalo ci sie za to dostac **{prize.XP}** punktow statusu spolecznego.");

           GivePrize(account, prize);
    }
        
        private double CountChance(int allCash, int placedCash)
        {
            double chance = 50;
            if (placedCash > allCash / 2)
                chance += ((double)placedCash - allCash / 2) / 6 / (double)allCash * 100;

            return chance;
        }

        private Prize DoDraw(int placedCash, double chance)
        {
            Random randomizer = new Random();
            double random = randomizer.Next(1, 101);

            Prize prize;
            prize.Cash = chance > random ? placedCash : placedCash * -1;
            prize.XP = chance > random ? placedCash / 10 : placedCash / 20;
            return prize;
        }

        private static void GivePrize(UserAccount account, Prize prize)
        {
            account.Cash += prize.Cash;
            account.XP += prize.XP;
            account.lastRoulette = DateTime.Now;
            UserAccounts.SaveAccounts();
        }
    }
}
