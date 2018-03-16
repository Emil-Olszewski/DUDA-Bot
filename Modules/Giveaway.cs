using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Modules
{
    static public class Giveaway
    {
        const int scale = 100;
        private static SocketGuildUser lastWinner;
        static Prize[] prizes =
        {
            new Prize("Skrzynka!", cases: 1, chance: 80),
            new Prize("Klucz!", keys: 1, chance: 50),
            new Prize("Skrzynka z kluczem!", keys: 1, cases: 1, chance: 30),
            new Prize("100 dudow!", cash: 100, chance: 20),
            new Prize("300 dudow!", cash: 300, chance: 10),
            new Prize("500 dudow!", cash: 500, chance: 0)
        };

        static public async Task Do(SocketGuild guild, SocketTextChannel channel)
        {
            SocketGuildUser user = GetRandomUser(guild);
            var account = UserAccounts.GetAccount(user);
            Prize prize = Prize.GetRandomPrize(prizes, scale);

            await channel.SendMessageAsync($"Gratuluje {user.Mention}." + $" Wygrales **{prize.Name}**");
            GivePrizeWinner(account, prize);
            lastWinner = user;
        }
        
        static private SocketGuildUser GetRandomUser(SocketGuild guild)
        {
            Random randomizer = new Random();
            int random;
            do
            {
                random = randomizer.Next(0, guild.Users.Count);
            } while (guild.Users.ElementAt(random).IsBot == true
                    || guild.Users.ElementAt(random).Status != Discord.UserStatus.Online
                    || guild.Users.ElementAt(random) == lastWinner);

            return guild.Users.ElementAt(random);
        }

        private static void GivePrizeWinner(UserAccount account, Prize prize)
        {
            account.Cash += prize.Cash;
            account.NumberOfKeys += (uint)prize.Keys;
            account.NumberOfCases += (uint)prize.Cases;
            UserAccounts.SaveAccounts();
        }

    }
}
