using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Modules
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("cash+")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task CashDotation(int amount, [Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.Cash += amount;
            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("xp+")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task XPDotation(int amount, [Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.XP += amount;
            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("t-reset")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task ResetTimes([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.last10Use = DateTime.Now - new TimeSpan(10, 0, 0, 0);
            account.last500Use = DateTime.Now - new TimeSpan(10, 0, 0, 0);
            account.lastRoulette = DateTime.Now - new TimeSpan(10, 0, 0, 0);
            account.lastTransfer = DateTime.Now - new TimeSpan(10, 0, 0, 0);
            account.lastCaseOpen = DateTime.Now - new TimeSpan(10, 0, 0, 0);
            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("reset")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task ResetUser([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var a = UserAccounts.GetAccount(target);
            a.Cash = 0;
            a.XP = 0;
            a.Level = 0;
            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("giveaway")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DoGiveaway()
        {
            await Giveaway.Do(Context.Guild, Context.Guild.GetTextChannel(Context.Channel.Id));
        }
    }
}
