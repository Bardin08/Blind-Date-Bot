using BlindDateBot.Commands;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Processors
{
    public class EventsProcessor
    {
        private readonly IConfiguration _config;

        private readonly TransactionProcessor _transactionProcessor;

        public EventsProcessor(ITelegramBotClient botClient, ILogger logger, IConfiguration config)
        {
            _config = config;
            _transactionProcessor = new TransactionProcessor(botClient, logger);

            EndDateCommand.DateEnd += DateEnd;
            NextDateCommand.DateFound += DateFound;
            ReportCommand.ReportInitiated += ReportInitiated;
            StartCommand.RegistrationInitiated += RegistrationInitiated;
            FeedbackCommamd.FeedbackTransactionInitiated += FeedbackTransactionInitiated;
        }

        private void DateEnd(string dateId)
        {
            TransactionsContainer.RemoveDate(dateId);
        }

        private void DateFound(DateTransactionModel transaction)
        {
            TransactionsContainer.AddDate(transaction);
        }

        private async void RegistrationInitiated(RegistrationTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            _transactionProcessor.Strategy = TransactionProcessStrategy.Registration;
            

            await _transactionProcessor.ProcessTransaction(
                new Message() { From = new User { Id = transaction.RecipientId } },
                                     transaction,
                                     new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }

        private async void FeedbackTransactionInitiated(FeedbackTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            _transactionProcessor.Strategy = TransactionProcessStrategy.Feedback;

            await _transactionProcessor.ProcessTransaction(
                new Message() { From = new User { Id = transaction.RecipientId } },
                                     transaction,
                                     new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }

        private async void ReportInitiated(ReportTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            _transactionProcessor.Strategy = TransactionProcessStrategy.Report;

            await _transactionProcessor.ProcessTransaction(
                new Message() { From = new User { Id = transaction.RecipientId } },
                                     transaction,
                                     new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }
    }
}
