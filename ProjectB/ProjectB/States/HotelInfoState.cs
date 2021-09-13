using ProjectB.Enums;
using ProjectB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class HotelInfoState : IState
    {
        private IHotelService _hotelService;

        public HotelInfoState(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        => Task.FromResult(State.HotelInfoState);

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.HotelInfoState);

        public Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        => Task.FromResult(State.HotelInfoState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId, int hotelId, string chechIn, string checkOut)
        {
            var hotel = await _hotelService.GetHotelDetailsById(hotelId, chechIn, checkOut);
            hotel.BookingLink = hotel.BookingLink.Replace("id", hotelId.ToString());
            hotel.BookingLink = hotel.BookingLink.Replace("checkindate", chechIn);
            hotel.BookingLink = hotel.BookingLink.Replace("checkoutdate", checkOut);
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"Name: {hotel.Name}",hotel.Name),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"PostalCode: {hotel.PostalCode}",hotel.PostalCode),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Price: {hotel.Price.ToString()}",hotel.Price.ToString()),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Address: {hotel.Address}",hotel.Address),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Map",hotel.StaticMapLink),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"StarRating: {hotel.StarRating.ToString()}",hotel.StarRating.ToString()),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("book",hotel.BookingLink),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Back","Back"),
                    },
            });

            var message = new Message();
            message.Text = "This is The Hotel Info";
            message.ReplyMarkup = inlineKeyboard;

            await botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: inlineKeyboard);
        }
    }
}
