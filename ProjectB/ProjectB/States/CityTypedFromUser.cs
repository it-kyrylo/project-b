using ProjectB.Enums;
using ProjectB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class CityTypedFromUser : IState
    {
        private IHotelService _hotelService;

        public CityTypedFromUser(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        => Task.FromResult(State.CityTypedFromUserState);

        public async Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            await BotSendMessage(botClient, message.Chat.Id, message.Text.ToString());
            return State.CityTypedFromUserState;
        }

        

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId, string cityName)
        {
            var hotels = await _hotelService.GetDestinationIdAsync(cityName);
            var buttons = new List<InlineKeyboardButton[]>();
            foreach (var hotel in hotels)
            {
                var button = new[]
                {
                    InlineKeyboardButton.WithCallbackData(hotel.HotelName,hotel.Id.ToString())
                };
                buttons.Add(button);
            }

            var inlineKeyboard = new InlineKeyboardMarkup(buttons);
            var message = new Message();
            message.Text = "Choose From Hotels";
            await botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: inlineKeyboard);
        }

        public Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        => Task.FromResult(State.CityTypedFromUserState);
    }
}
