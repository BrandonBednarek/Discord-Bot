using Discord.WebSocket;
using System.Threading.Tasks;

namespace Discord_Bot.Services
{
    class UserJoinedService
    {
        private readonly DiscordSocketClient _client;

        // DiscordSocketClient is injected automatically from the IServiceProvider
        public UserJoinedService(DiscordSocketClient client)
        {
            _client = client;

            _client.UserJoined += UserJoinedAsync;
        }

        private async Task UserJoinedAsync(SocketGuildUser user)
        {
            var channel = _client.GetChannel(695053048887771137) as SocketTextChannel;
            await channel.SendMessageAsync($"AYOWADDUP it's {user.Username}"); // Welcomes the new user
        }
    }
}
