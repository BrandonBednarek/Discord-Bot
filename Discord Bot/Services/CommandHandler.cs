using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    internal class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            IServiceProvider provider)
        {
            _client = client;
            _commands = commands;
            _provider = provider;

            _client.MessageReceived += ReceivedAsync;
        }
        private async Task ReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            /* convert celsius to f
            var regex = new Regex(@"\b(\d{1,3})(?!%|a|p)\b");
            if (regex.IsMatch(message.Content))
            {
                var matches = regex.Matches(message.Content);

                foreach (Match m in matches)
                {
                    await message.Channel.SendMessageAsync(m.Value);
                }
            }*/

            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("pong!");
        }
    }
}