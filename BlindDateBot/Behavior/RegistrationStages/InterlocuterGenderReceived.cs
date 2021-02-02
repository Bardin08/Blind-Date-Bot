﻿using System.Text;
using System.Threading.Tasks;

using BlindDateBot.Data.Interfaces;
using BlindDateBot.Domain.Models.Enums;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlindDateBot.Behavior.RegistrationStages
{
    internal class InterlocuterGenderReceived : IRegistrationTransactionState
    {
        public async Task ProcessTransaction(
            Message message,
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDatabase db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            ValidateInputAndUpdateUserModel(message,
                                            currentTransaction,
                                            botClient,
                                            logger,
                                            db);

            // TODO: Generate normal message
            var sb = new StringBuilder();
            sb.Append(string.Format(Messages.ConfirmData,
                                    currentTransaction.User.Gender.ToString(),
                                    currentTransaction.User.InterlocutorGender.ToString()));

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId, sb.ToString(), replyMarkup: CreateReplyKeyboard());
            currentTransaction.TransactionState = new ConfirmationReceived();
        }

        private static async void ValidateInputAndUpdateUserModel(
            Message message,
            RegistrationTransactionModel transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDatabase db)
        {
            if (message?.Text == null || !int.TryParse(message.Text, out int genderId))
            {
                await botClient.SendTextMessageAsync(transaction.RecepientId, Messages.SomethingWentWrong);

                transaction.TransactionState = new RegistrationInitiated();
                await transaction.TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                return;
            }

            if (genderId == 0)
            {
                transaction.User.InterlocutorGender = Gender.Male;
            }
            else if (genderId == 1)
            {
                transaction.User.InterlocutorGender = Gender.Female;
            }

        }
    
        private static InlineKeyboardMarkup CreateReplyKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    Text = Messages.Yes,
                    CallbackData = "1"
                },
                new InlineKeyboardButton()
                {
                    Text = Messages.No,
                    CallbackData = "0"
                }
            });
        }
    }
}