using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Discord_Bot.Services
{
    class ReactionService
    {
        private readonly DiscordSocketClient _client;

        // DiscordSocketClient is injected automatically from the IServiceProvider
        public ReactionService(DiscordSocketClient client)
        {
            _client = client;

            _client.ReactionAdded += ReactionAddedAsync;
        }

        private async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originalChannel, SocketReaction reaction)
        {
            // Delete Message if it has a score < 5
            int score = 5;
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message != null && reaction.User.IsSpecified)
            {
                if (reaction.Emote.Name.Equals("Patboo"))
                {
                    int booCount = 0;

                    foreach (var item in message.Reactions.Keys)
                    {
                        if (item.Name == "Patboo")
                            booCount++;
                        else
                            booCount--;
                    }

                    if (booCount >= score)
                    {
                        await message.DeleteAsync();
                    }
                }
            }
        }
    }
}
