using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Discord_BOT.Modules;

namespace Discord_BOT.Core
{
    internal static class RepeatingTime
    {
        private static Timer timer;
        private static SocketTextChannel channel;
        private static SocketGuild guild;

        private const long DUDA_SERWER = 369182365978984450;
        private const long DUDA_CHANNEL = 422406927914500126;

        internal static Task StartTimer()
        {
            guild = Global.Client.GetGuild(DUDA_SERWER);
            channel = guild.GetTextChannel(DUDA_CHANNEL);
            timer = new Timer()
            {
                Interval = 60000*5,
                AutoReset = true,
                Enabled = true
            };

            timer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            await Giveaway.Do(guild, channel);     
        }
    }
}
