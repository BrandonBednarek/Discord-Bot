using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    class LoggingService
    {
        private readonly DiscordSocketClient _client;

        // DiscordSocketClient is injected automatically from the IServiceProvider
        public LoggingService(DiscordSocketClient client)
        {
            _client = client;

            _client.Log += LogAsync;
        }

        private Task LogAsync(LogMessage log) // The Log event starts the logger to the console
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
