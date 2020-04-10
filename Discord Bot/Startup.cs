﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    class Startup
    {
        public Startup(string[] args)
        {
            /*
            _config = new DiscordSocketConfig { MessageCacheSize = 1000 };      // enable message cache
            _client = new DiscordSocketClient(_config);                         // create client

            _client.Log += LogAsync; // start the logger
            _client.Ready += ReadyAsync; // log in and start async
            // _client.MessageReceived += Com.ReceivedAsync; // run when a message is received
            _client.ReactionAdded += ReactionAddedAsync; // run when a reaction is added
            _client.UserJoined += UserJoinedAsync; // run when a reaction is added
            */
        }

        public static async Task MainAsync(string[] args)   // This event logs the client in and starts async
        {
            var startup = new Startup(args);
            await startup.MainAsync();
        }

        public async Task MainAsync()
        {
            var services = new ServiceCollection();     // Create a new instance of a service collection
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();                     // Build the service provider
            provider.GetRequiredService<CommandHandler>();                      // Start the command handler service
            await provider.GetRequiredService<StartupService>().StartAsync();   // Start the startup service

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                       // Add discord to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                MessageCacheSize = 1000             // Cache 1,000 messages per channel
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {                                       // Add the command service to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
            }))
            .AddSingleton<CommandHandler>()         // Add the command handler to the collection
            .AddSingleton<StartupService>();        // Add startupservice to the collection
        }

        /*
        private Task LogAsync(LogMessage log) // The Log event starts the logger to the console
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync() // The Ready event indicates that the client has opened a connection and it is now safe to access the cache.
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
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
            var channel = _client.GetChannel(695053048887771137) as SocketTextChannel;
            await channel.SendMessageAsync($"AYOWADDUP it's {user.Username}"); // Welcomes the new user
        }

        // This is not the recommended way to write a bot - consider reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            var regex = new Regex(@"\b(\d{1,3})(?!%|a|p)\b");
            if (regex.IsMatch(message.Content))
            {
                var matches = regex.Matches(message.Content);

                foreach (Match m in matches)
                {
                    await message.Channel.SendMessageAsync(m.Value);
                }
            }

            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("pong!");
        }
        */
    }
}