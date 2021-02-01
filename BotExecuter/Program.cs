using System;
using System.IO;

using BlindDateBot;
using BlindDateBot.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace BotExecuter
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfiguration(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, s) =>
                {
                    s.AddSingleton<IBlindDateBotClient>(new BlindDataBotClient(builder.Build()));
                    s.AddSingleton<BlindDateBot.BlindDateBot>();
                })
                .UseSerilog()
                .Build();

            var bot = ActivatorUtilities.CreateInstance<BlindDateBot.BlindDateBot>(host.Services);
            bot.Execute();
        }

        private static void BuildConfiguration(ConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
