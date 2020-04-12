using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Discord_Bot
{
    internal class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider provider)
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

            // Bot should only reply to me
            if (message.Author.Id != 147163300818321408)
                return;

            // Bot replies to !ping with !pong
            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("pong!");

            // Converts Celsius in messages to Fahrenheit
            var regex = new Regex(@"-?\d+(?:\.\d+)?(?:\s*°\s*c)?");
            if (regex.IsMatch(message.Content))
            {
                var matches = regex.Matches(message.Content);

                int celsius = Int32.Parse(matches[0].Value);
                int fah = (celsius * 9) / 5 + 32;

                foreach (Match m in matches)
                {
                    await message.Channel.SendMessageAsync(fah + "F");
                }
            }
        }
    }
}