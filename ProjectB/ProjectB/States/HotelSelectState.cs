using ProjectB.Enums;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class HotelSelectState : IState
    {
        private int hotelId;
        public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            this.hotelId = int.Parse(callbackQuery.Data.ToString());
            await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
            return State.HotelSelectState;
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.HotelSelectState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Show CheckIn Date"),
                    }
            });

            await botClient.SendTextMessageAsync(chatId, "Select", replyMarkup: inlineKeyboard);
        }
    }
}
