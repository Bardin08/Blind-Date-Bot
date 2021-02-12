using System.Collections.Generic;
using System.Linq;

using BlindDateBot.Models;

namespace BlindDateBot
{
    public static class TransactionsContainer
    {
        private static readonly List<object> _transactions = new();
        private static readonly List<DateTransactionModel> _dates = new();

        public static void RemoveDate(string dateId)
        {
            lock (_dates)
            {
                _dates.RemoveAll(d => d.TransactionId == dateId);
            }
        }

        public static void AddDate(DateTransactionModel transaction)
        {
            lock (_dates)
            {
                if (transaction != null)
                {
                    _dates.Add(transaction);
                }
            }
        }

#nullable enable
        public static DateTransactionModel? GetDateTransactionByRecipientId(int recipientId)
        {
            lock (_dates)
            {
                return _dates.FirstOrDefault(d => d.Date.FirstUser.TelegramId == recipientId
                                               || d.Date.SecondUser.TelegramId == recipientId);
            }
        }

        public static object? GetTransactionByRecipientId(int recipientId)
        {
            lock (_transactions)
            {
                _transactions.RemoveAll(t => (t as BaseTransactionModel).IsComplete);

                return _transactions.FirstOrDefault(t => (t as BaseTransactionModel)!.RecipientId.Equals(recipientId));
            }
        }
#nullable disable

        public static void AddTransaction(object transaction)
        {
            lock (_transactions)
            {
                if (transaction != null)
                {
                    _transactions
                        .RemoveAll(t => (t as BaseTransactionModel).RecipientId == (transaction as BaseTransactionModel).RecipientId);

                    _transactions.Add(transaction);
                }
            }
        }

        public static bool DateForUserExists(int userId)
        {
            lock(_dates)
            {
                return _dates.FirstOrDefault(d => d.Date.FirstUser.TelegramId == userId 
                                               || d.Date.SecondUser.TelegramId == userId) != null;
            }
        }
    }
}
