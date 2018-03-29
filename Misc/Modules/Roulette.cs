using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Misc.Modules
{
    public class Roulette : ModuleBase<SocketCommandContext>
    {
        private struct Prize { public long Xp; public long Cash; };

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
        public async Task PlayRoulette(long placedCash)
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (placedCash > account.Cash)
            {
                await Context.Channel.SendMessageAsync($":warning: Kanciarza chcesz kantowac?");
                return;
            }

            if (placedCash < Config.Bot.MinimalRoulette)
            {
                await Context.Channel.SendMessageAsync($":warning: Minimalna kwota zakladu to " +
                $":small_orange_diamond: **{Config.Bot.MinimalRoulette}** Dudow.");
                return;
            }

            double chance = CountChance(account.Cash, placedCash);
            var prize = DoDraw(placedCash, chance);

            if (prize.Cash > 0)
                await Context.Channel.SendMessageAsync($":white_check_mark: {Context.User.Mention} +:small_orange_diamond:" +
                $"{prize.Cash * 1} Dudow +:small_blue_diamond:{prize.Xp} XP");
            else
                await Context.Channel.SendMessageAsync($":x: {Context.User.Mention} -:small_orange_diamond:" +
                $"{prize.Cash * -1} Dudow +:small_blue_diamond:{prize.Xp} XP");

            GivePrize(account, prize);
        }

        private static double CountChance(long allCash, long placedCash)
        {
            double chance = 50;
            if (placedCash > allCash / 2)
                chance += (placedCash - (double)allCash / 2) / 6 / allCash * 100;

            return chance;
        }

        private static Prize DoDraw(long placedCash, double chance)
        {
            var randomizer = new Random();
            double random = randomizer.Next(1, 101);

            Prize prize;
            prize.Cash = chance > random ? placedCash : placedCash * -1;
            prize.Xp = chance > random ? placedCash / 10 : placedCash / 20;
            return prize;
        }

        private static void GivePrize(UserAccount account, Prize prize)
        {
            account.Cash += prize.Cash;
            account.Xp += prize.Xp;
            UserAccounts.SaveAccounts();
        }
    }
}
