using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot
{
    class Program
    {
        public static Task Main(string[] args) => Startup.MainAsync(args);
    }
}
