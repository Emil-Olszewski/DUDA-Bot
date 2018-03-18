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
            new Prize("1000 dudow!", cash: 1000, chance: 85),
            new Prize("Skrzynka z kluczem!", keys: 1, cases: 1, chance: 65),
            new Prize("Klucz!", keys: 1, chance: 45),
            new Prize("500 dudow!", cash: 500, chance: 30),
            new Prize("300 dudow!", cash: 300, chance: 15),
            new Prize("Skrzynka!", cases: 1, chance: 0)
        };

        static public async Task Do(SocketGuild guild, SocketTextChannel channel)
        {
            if (!IsAtLeastTwoUsersOnline(guild))
                return;

            SocketGuildUser user = GetRandomUser(guild);
            var account = UserAccounts.GetAccount(user);
            Prize prize = Prize.GetRandomPrize(prizes, scale);

            await channel.SendMessageAsync($"Gratuluje {user.Mention}." + $" Wygrales **{prize.Name}**");
            GivePrize(account, prize);
            lastWinner = user;
        }

        private static bool IsAtLeastTwoUsersOnline(SocketGuild guild)
        {
            int numberOfUsersOnline = 0;
            foreach (var user in guild.Users)
            {
                if (user.Status != Discord.UserStatus.Offline)
                    numberOfUsersOnline++; 

                if (numberOfUsersOnline >= 2) return true;
            }

            return false;
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

        private static void GivePrize(UserAccount account, Prize prize)
        {
            account.Cash += prize.Cash;
            account.NumberOfKeys += (uint)prize.Keys;
            account.NumberOfCases += (uint)prize.Cases;
            UserAccounts.SaveAccounts();
        }

        public static Task RewardAllActiveUsers(SocketGuild guild)
        {
            foreach(var user in guild.Users)
            {
                if (user.Status != Discord.UserStatus.Offline)
                {
                    var account = UserAccounts.GetAccount(user);
                    account.Cash += Config.Bot.cash5Min;
                }
            }

            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }
    }
}
