using Discord;
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

            // regex if message is a link
            var link = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
            // regex temp format
            var temp = new Regex(@"-?\d+(?:\.\d+)?(?:\s*°)?[cCfF]");

            // Converts Celsius in messages to Fahrenheit
            if (!link.IsMatch(message.Content))
            {
                if (temp.IsMatch(message.Content))
                {
                    var matches = temp.Matches(message.Content);
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
            }

            // Only commands that start with ! as a prefix
            if (message.Content[0] != '!')
                return;

            // The bot should only read messages from #bot-commands or #bot-testing, else delete
            if (message.Channel.Id != 747591343546695710 && message.Channel.Id != 694062245566611517)
            {
                const int delay = 5000;
                var m = await message.Channel.SendMessageAsync($"Use commands in #bot-commands. _This message will be deleted in {delay / 1000} seconds._");
                await Task.Delay(delay);
                await m.DeleteAsync();
                await (message.Channel as SocketTextChannel).DeleteMessageAsync(message);
                return;
            }


            //// Bot replies to !ping with !pong
            //if (message.Content == "!ping")
            //{
            //    await message.Channel.SendMessageAsync("pong!"); return;
            //}

            if (message.Content.Contains("!bulkclear") && message.Author.Id == 147163300818321408)
            {
                try
                {
                    // see if number in clear
                    int toDelete = Int32.Parse(Regex.Match(message.Content, @"\d+").Value);

                    var messages = await message.Channel.GetMessagesAsync(toDelete + 1).FlattenAsync();

                    await (message.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                    const int delay = 5000;
                    var m = await message.Channel.SendMessageAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
                    await Task.Delay(delay);
                    await m.DeleteAsync();

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