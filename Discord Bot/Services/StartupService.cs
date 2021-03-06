﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Discord_Bot
{
    public class StartupService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfiguration, and IServiceProvider are injected automatically from the IServiceProvider
        public StartupService(DiscordSocketClient client, CommandService commands, IConfiguration config, IServiceProvider provider)
        {
            _client = client;
            _commands = commands;
            _config = config;
            _provider = provider;

            _client.Ready += ReadyAsync;
        }

        public async Task StartAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _config["Discord:Token"]);
            await _client.StartAsync();

            // Load commands and modules into the command service
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        // The Ready event indicates that the client has opened a connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }
    }
}