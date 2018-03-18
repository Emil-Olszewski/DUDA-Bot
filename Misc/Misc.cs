using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Discord_BOT.Misc
{
    public class Misc
    {
        public static bool IsUserRankOwner(SocketGuildUser user, string targetRoleName)
        {
            var result = from r in user.Guild.Roles where r.Name == targetRoleName select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }

        public static bool WasRequiredTimeElapsed(DateTime date, TimeSpan time)
        {
            DateTime requiredDate = date.Add(time);
            if (DateTime.Now >= requiredDate) return true;
            return false;
        }
    }
}
