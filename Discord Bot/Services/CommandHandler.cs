using Discord;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Discord_Bot
{
    internal class CommandHandler
    {
        private readonly DiscordSocketClient _client;

        // DiscordSocketClient is injected automatically from the IServiceProvider
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _client.MessageReceived += ReceivedAsync;
        }

        private async Task ReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            // checks if message is a link
            var link = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
            if (link.IsMatch(message.Content))
            {
                return;
            }

            // Converts Celsius in messages to Fahrenheit
            var regex = new Regex(@"-?\d+(?:\.\d+)?(?:\s*°)?[cCfF]");
            if (regex.IsMatch(message.Content))
            {
                var matches = regex.Matches(message.Content);
                try
                {
                    string num = matches[0].Value.ToUpper().Replace(@"C", string.Empty);

                    int celsius = Int32.Parse(num);
                    int fahrenheit = (celsius * 9) / 5 + 32;

                    await message.Channel.SendMessageAsync(num + "°C / " + fahrenheit + "°F");
                }
                catch
                {
                    string num = matches[0].Value.ToUpper().Replace(@"F", string.Empty);

                    int fahrenheit = Int32.Parse(num);
                    int celsius = (fahrenheit - 32) * 5 / 9;

                    await message.Channel.SendMessageAsync(num + "°F / " + celsius + "°C");
                }
            }

            // Only allow images or hyperlinks in the "Memes" channel
            /*var channel = _client.GetChannel(694062245566611517) as SocketTextChannel;
            string deleteMessage = "This channel is designated for images or links only. Use the other channels for conversations.";
            if (message.Channel == channel)
            {
                if (message.Attachments.Count < 1)
                    await message.Channel.SendMessageAsync(deleteMessage);
                    await message.DeleteAsync();
            }*/

            // Only commands that start with ! as a prefix
            if (message.Content[0] != '!')
                return;


            // Bot replies to !ping with !pong
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("pong!"); return;
            }

            if (message.Content.Contains("!clear") && message.Author.Id == 147163300818321408)
            {
                try
                {
                    int toDelete = Int32.Parse(message.Content.Remove(0, 6).Trim());
                    var messages = await message.Channel.GetMessagesAsync(toDelete + 1).FlattenAsync(); //defualt is 100
                    await (message.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
                    return;
                }
                catch
                {
                    return;
                }
            }

            if (message.Content == "!watch")
            {
                await message.Channel.SendMessageAsync("https://www.watch2gether.com/rooms/mg61hq840mnib7vfib"); return;
            }
        }
    }
}