using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()        // Create a new instance of the config builder
                .SetBasePath(AppContext.BaseDirectory)      // Specify the default location for the config file
                .AddUserSecrets<Startup>();                 // Add user secrets
            Configuration = builder.Build();                // Build the configuration
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
            provider.GetRequiredService<LoggingService>();                      // Start the logging service
            provider.GetRequiredService<ReactionService>();                     // Start the reaction service
            provider.GetRequiredService<UserJoinedService>();                   // Start the user joined service

            await provider.GetRequiredService<StartupService>().StartAsync();   // Start the startup service

            await Task.Delay(-1); // Block this task until the program is closed.
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
            .AddSingleton<CommandHandler>()         // Add CommandHandler to the collection
            .AddSingleton<LoggingService>()         // Add LoggingService to the collection
            .AddSingleton<ReactionService>()        // Add ReactionService to the collection
            .AddSingleton<StartupService>()         // Add StartupService to the collection
            .AddSingleton<UserJoinedService>()      // Add UserJoinedService to the collection
            .AddSingleton(Configuration);           // Add configuration to the collection
        }
    }
}
