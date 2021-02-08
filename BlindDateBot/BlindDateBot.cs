using System.Collections.Generic;
using System.Linq;
using System.Threading;

using BlindDateBot.Commands;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;
using BlindDateBot.Options;
using BlindDateBot.Processors;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace BlindDateBot
{
    public class BlindDateBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BlindDateBotOptions _options;

        private readonly ILogger<BlindDateBot> _logger;
        private readonly IConfiguration _config;

        private TelegramUdpateProcessor _updateProcessor;

        public BlindDateBot(IBlindDateBotClient botClient, ILogger<BlindDateBot> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _botClient = botClient.BotClient;
            _options = botClient.Options;

            _updateProcessor = new(_botClient, logger, config);

            foreach (var date in LoadDatesFromDatabase())
            {
                TransactionsContainer.AddDate(new DateTransactionModel(date));
            }
        }

        public void Execute()
        {
            _botClient.OnUpdate += UpdateReceived;

            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private void UpdateReceived(object sender, UpdateEventArgs e)
        {
            _updateProcessor.Process(e.Update); 
        }

        private List<DateModel> LoadDatesFromDatabase()
        {
            var dbContext = new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]);

            return dbContext.Dates
                .Include(d => d.Messages)
                .Include(d => d.FirstUser)
                .Include(d => d.SecondUser)
                .Where(d => d.IsActive == true).ToList();
        }

    }
}