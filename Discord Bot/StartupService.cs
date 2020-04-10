using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Discord_Bot
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient client,
            CommandService commands)
        {
            _provider = provider;
            _client = client;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
            await _client.StartAsync();
        }
    }
}