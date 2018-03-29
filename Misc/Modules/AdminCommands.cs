using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Misc.Modules
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("cash+")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task CashDotation(int amount, [Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            var target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.Cash += amount;

            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("xp+")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task XpDotation(int amount, [Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            var target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.Xp += amount;

            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("reset")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task ResetUser([Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            var target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.Cash = 0;
            account.Xp = 0;
            account.Level = 0;

            UserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }

        [Command("vip")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task GiveUserVip([Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            var target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);

            account.Vip = !account.Vip;

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
