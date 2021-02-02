﻿using System.Threading.Tasks;

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
    internal class GenderReceived : IRegistrationTransactionState
    {
        public async Task ProcessTransaction(
            Message message,
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDatabase db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            ValidateInputAndUpdateModel(message,
                                        currentTransaction,
                                        botClient,
                                        logger,
                                        db);

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId,
                                                 Messages.SelectInterlocuterGender,
                                                 replyMarkup: CreateReplyKeyboard());

            currentTransaction.TransactionState = new InterlocuterGenderReceived();
        }

        private static async void ValidateInputAndUpdateModel(
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
                transaction.User.Gender = Gender.Male;
            }
            else if (genderId == 1)
            {
                transaction.User.Gender = Gender.Female;
            }
        }

        private static InlineKeyboardMarkup CreateReplyKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    CallbackData = "0",
                    Text = Messages.Male
                },
                new InlineKeyboardButton()
                {
                    CallbackData = "1",
                    Text = Messages.Female
                }
            });
        }
    }
}