using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord_BOT.Core;
using Discord;
using Discord.WebSocket;

namespace Discord_BOT
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        private static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.Bot.Token is null || Config.Bot.Token == "") return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            _client.Ready += RepeatingTime.StartTimer;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            Global.Client = _client;
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
