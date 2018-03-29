using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Misc.Modules.CasesSystem;

namespace Discord_BOT.Misc.Modules
{
    public static class Giveaway
    {
        private static SocketGuildUser _lastWinner;
        private static readonly List<ulong> PreviousUserIDs = new List<ulong>();

        private static readonly Prize[] Prizes =
         {
            new Prize(@case: Cases.List.Find(x => x.ID == 103), chance: 990),
            new Prize(@case: Cases.List.Find(x => x.ID == 102), chance: 980),
            new Prize(@case: Cases.List.Find(x => x.ID == 101), chance: 890),
            new Prize(@case: Cases.List.Find(x => x.ID == 100), chance: 640),
            new Prize(key: new Key(Item.QUALITY.GOLD, 3), chance: 630),
            new Prize(key: new Key(Item.QUALITY.GOLD, 2), chance: 610),
            new Prize(key: new Key(Item.QUALITY.GOLD, 1), chance: 580),
            new Prize(key: new Key(Item.QUALITY.GOLD, 0), chance: 540),
            new Prize(key: new Key(Item.QUALITY.SILVER, 3), chance: 520),
            new Prize(key: new Key(Item.QUALITY.SILVER, 2), chance: 480),
            new Prize(key: new Key(Item.QUALITY.SILVER, 1), chance: 420),
            new Prize(key: new Key(Item.QUALITY.SILVER, 0), chance: 340),
            new Prize(key: new Key(Item.QUALITY.BRONZE, 4), chance: 290),
            new Prize(key: new Key(Item.QUALITY.BRONZE, 3), chance: 240),
            new Prize(key: new Key(Item.QUALITY.BRONZE, 2), chance: 170),
            new Prize(key: new Key(Item.QUALITY.BRONZE, 1), chance: 90),
            new Prize(key: new Key(Item.QUALITY.BRONZE, 0))
        };

        public static async Task Do(SocketGuild guild, SocketTextChannel channel)
        {
            if (!IsAtLeastTwoUsersOnline(guild))
                return;

            var user = GetRandomUser(guild);
            var account = UserAccounts.GetAccount(user);
            var prize = Prize.GetRandomPrize(Prizes.ToArray());

            await channel.SendMessageAsync($"**[GIVEAWAY]** {user.Mention}" + $" wygrales **{prize.Name}**.");
            prize.GiveAndReturnProfit(account);
            UserAccounts.SaveAccounts();
            _lastWinner = user;
        }

        private static bool IsAtLeastTwoUsersOnline(SocketGuild guild)
        {
            var numberOfUsersOnline = 0;
            foreach (var user in guild.Users)
            {
                if (user.Status != UserStatus.Offline && user.IsBot == false)
                    numberOfUsersOnline++;

                if (numberOfUsersOnline > 1) return true;
            }

            return false;
        }

        private static SocketGuildUser GetRandomUser(SocketGuild guild)
        {
            var randomizer = new Random();
            int random;

            do
            {
                random = randomizer.Next(0, guild.Users.Count);
            } while (guild.Users.ElementAt(random).IsBot
                    || guild.Users.ElementAt(random).Status == UserStatus.Offline
                    || guild.Users.ElementAt(random) == _lastWinner);

            return guild.Users.ElementAt(random);
        }

        public static Task RewardAllActiveUsers(SocketGuild guild)
        {
            foreach (var user in guild.Users)
            {
                if (user.Status == UserStatus.Offline) continue;
                var account = UserAccounts.GetAccount(user);
                account.Cash += Config.Bot.Cash5Min;

                if (PreviousUserIDs.Exists(x => x == user.Id))
                    account.TimeSpendOnDiscord += new TimeSpan(0, 5, 0);
                else
                    PreviousUserIDs.Add(user.Id);
            }

            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }
    }
}
