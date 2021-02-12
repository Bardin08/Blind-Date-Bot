using System;
using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Strategies
{
    public class ReportProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = (transaction as ReportTransactionModel);

            try
            {
                if (currentTransaction.Message != null && transaction != null)
                {
                    await currentTransaction.TransactionState.ProcessTransaction(transaction, botClient, logger, db);
                }
            }
            catch (Exception ex)
            {
                var transactionModelState = currentTransaction.ToString();
                
                logger.LogError($"Exception occurred. Source: {ex.Source}, Message: {ex.Message}, TransactionModel: {transactionModelState}, StackTrace: {ex.StackTrace}");
            }
        }
    }
}
