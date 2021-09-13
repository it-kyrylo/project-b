using ProjectB.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class MainState : IState
    {
        public Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        => Task.FromResult(State.MainState);

        public async Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            await BotSendMessage(botClient, message.Chat.Id);
            return State.MainState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Help","Help"),
                    InlineKeyboardButton.WithCallbackData("Hotels", "Please Type The City You Wont To Visit"),
                },
            });
            var message = new Message();
            message.Text = "Welcome Please choose from buttons below";
            await botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: inlineKeyboard);
        }
    }
}
