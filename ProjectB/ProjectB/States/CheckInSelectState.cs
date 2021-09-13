﻿using ProjectB.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class CheckInSelectState : IState
    {
        public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var checkIn = callbackQuery.Data.ToString();
            await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
            return State.CheckInSelectState;
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.CheckInSelectState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Show CheckOut Dates"),
                    }
            });

            await botClient.SendTextMessageAsync(chatId, "Select", replyMarkup: inlineKeyboard);
        }
    }
}