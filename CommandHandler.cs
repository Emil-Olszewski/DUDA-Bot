using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_BOT
{
    internal class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage msg)) return;
            var context = new SocketCommandContext(_client, msg);
            var argPos = 0;
            if (msg.HasStringPrefix(Config.Bot.CmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    Console.WriteLine(result.Error);
            }
        }
    }
}
