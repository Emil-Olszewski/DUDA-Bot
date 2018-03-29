using System;
using System.Linq;
using Discord.WebSocket;

namespace Discord_BOT.Misc
{
    public class Checker
    {
        public static bool IsUserRankOwner(SocketGuildUser user, string targetRoleName)
        {
            ulong roleId = user.Guild.Roles.ToList().Find(x => x.Name == targetRoleName).Id;
            if (roleId == 0) return false;
            var targetRole = user.Guild.GetRole(roleId);
            return user.Roles.Contains(targetRole);
        }

        public static bool WasRequiredTimeElapsed(DateTime date, TimeSpan time)
        {
            var requiredDate = date.Add(time);
            return DateTime.Now >= requiredDate;
        }
    }
}
