using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot
{
    class Program
    {
        private readonly DiscordSocketConfig _config;
        private readonly DiscordSocketClient _client;

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public Program()
        {
            _config = new DiscordSocketConfig { MessageCacheSize = 100 };
            _client = new DiscordSocketClient(_config);

            _client.Log += LogAsync; // start the logger
            _client.Ready += ReadyAsync; // Log in and start async
            _client.MessageReceived += MessageReceivedAsync; // run when a message is received
            _client.ReactionAdded += ReactionAddedAsync; // run when a reaction is added
            _client.UserJoined += UserJoinedAsync; // run when a reaction is added
        }

        // This event deletes a message when a reaction is added to it at a certain threshold
        private async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originalChannel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message != null && reaction.User.IsSpecified)
            {
                Console.WriteLine($"{reaction.User.Value} just added a reaction '{reaction.Emote}' " + $"to {message.Author}'s message ({message.Id}).");
                Console.WriteLine(reaction.Emote.Name);
                if (reaction.Emote.Name.Equals("Patboo") && message.Reactions.Count >= 2)
                {
                    Console.WriteLine("Deleting message");
                    await message.DeleteAsync();
                }
            }
        }

        // This event welcomes the user when they join the server
        private async Task UserJoinedAsync(SocketGuildUser user)
        {
            var channel = _client.GetChannel(694033830654509170) as SocketTextChannel;
            await channel.SendMessageAsync($"AYOWADDUP it's {user.Username}"); // Welcomes the new user
        }

        // This event logs the client in and starts async
        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        // The Log event starts the logger to the console
        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        // This is not the recommended way to write a bot - consider reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            /*
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
