using System;
using System.Text;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class ReportProcessStrategy : ITransactionProcessingStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            try
            {
                if (message != null && transaction != null)
                {
                    await (transaction as ReportTransactionModel).TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                }
            }
            catch (Exception ex)
            {
                var transactionModelState = (transaction as ReportTransactionModel).ToString();
                
                logger.LogError($"Exception occurred. Source: {ex.Source}, Message: {ex.Message}, TransactionModel: {transactionModelState}, StackTrace: {ex.StackTrace}");
            }
        }
    }
}
