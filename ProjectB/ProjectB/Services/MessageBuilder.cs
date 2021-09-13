using ProjectB.Handlers;
using ProjectB.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.Services
{
    public class MessageBuilder : IMessageBuilder
    {
        private IHotelService _hotelService;
        private ITelegramBotClient _telegramBotClient;


        public MessageBuilder(IHotelService hotelService, ITelegramBotClient telegramBotClient)
        {
            _hotelService = hotelService;
            _telegramBotClient = telegramBotClient;
        }

        public Message MainState()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                 {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Help","Help"),
                        InlineKeyboardButton.WithCallbackData("Hotels", "Please Type The City You Wont To Visit"),
                    },
                    // second row
                });

            var message = new Message();
            message.Text = "Welcome Please choose from buttons below";
            message.ReplyMarkup = inlineKeyboard;
            return message;
        }

        public async Task<Message> HotelsToButtons(string cityName)
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
            message.ReplyMarkup = inlineKeyboard;
            return message;
        }
        public Message CheckInDate()
        {
            var message = new Message();
            var choosedate = DateTime.UtcNow;
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(choosedate.ToString("yyyy-MM-dd"), choosedate.ToString("yyyy-MM-dd")),
                    }
            });
            message.Text = "Choose checkin date";
            message.ReplyMarkup = inlineKeyboard;
            return message;
        }

        public Message CheckOutDate()
        {
            var buttons = new List<InlineKeyboardButton[]>();
            var choosedate = DateTime.UtcNow;
            for (int i = 1; i <= 7; i++)
            {
                choosedate = choosedate.AddDays(i);
                var button = new[]
                {
                    InlineKeyboardButton.WithCallbackData(choosedate.ToString("yyyy-MM-dd"),choosedate.ToString("yyyy-MM-dd"))
                };

                buttons.Add(button);
            }
            var inlineKeyboard = new InlineKeyboardMarkup(buttons);

            var message = new Message();
            message.Text = "Choose How many days u will stay";
            message.ReplyMarkup = inlineKeyboard;
            return message;
        }
        
        public async Task<Message> HotelInfo(int hotelId, string checkIn, string checkOut)
        {
            var hotel = await _hotelService.GetHotelDetailsById(hotelId, checkIn, checkOut);
            hotel.BookingLink = hotel.BookingLink.Replace("id", hotelId.ToString());
            hotel.BookingLink = hotel.BookingLink.Replace("checkindate", checkIn);
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
            return message;
        }
    }
}
