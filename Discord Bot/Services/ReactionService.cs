using Discord;
using Discord.WebSocket;
using System;
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
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message != null && reaction.User.IsSpecified)
            {
                Console.WriteLine($"{reaction.User.Value} just added a reaction '{reaction.Emote}' " + $"to {message.Author}'s message ({message.Id}).");
                Console.WriteLine(reaction.Emote.Name);
                if (reaction.Emote.Name.Equals("Patboo") && message.Reactions.Count >= 1)
                {
                    int booCount = 0;

                    foreach (var item in message.Reactions.Keys)
                    {
                        if (item.Name == "Patboo")
                            booCount++;
                    }

                    if (booCount >= 1)
                    {
                        await message.DeleteAsync();
                    }
                }
            }
        }
    }
}
