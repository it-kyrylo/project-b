using ProjectB.Enums;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectB.States
{
    public class HotelSelectState : IState
    {
        private int hotelid;
        public Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            this.hotelid = int.Parse(callbackQuery.Data.ToString());
            return Task.FromResult(State.HotelSelectState);
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.HotelSelectState);

        public Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        => Task.FromResult(State.HotelSelectState);
    }
}
