using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Discord_BOT.Misc.Modules;

namespace Discord_BOT.Core
{
    internal static class RepeatingTime
    {
        private static Timer Timer;
        private static SocketTextChannel Channel;
        private static SocketGuild Guild;

        internal static Task StartTimer()
        {
            Guild = Global.Client.GetGuild(Config.Bot.ServerId);
            Channel = Guild.GetTextChannel(Config.Bot.GiveawayChannelId);
            Timer = new Timer
            {
                Interval = 60000 * 5,
                AutoReset = true,
                Enabled = true
            };

            Timer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            await Giveaway.Do(Guild, Channel);
            await Giveaway.RewardAllActiveUsers(Guild);
        }
    }
}
