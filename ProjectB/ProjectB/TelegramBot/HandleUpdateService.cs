using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.TelegramBot

{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient botClient;
        private static List<Hotels> hotels = new List<Hotels>();
        private static bool townChosen = false;
        private static bool hotelChosen = false;
        private static bool searchHotel = false;


        public static void FillHotels()
        {
            hotels.Add(new Hotels(1, "Plovdiv", "Novotel", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(2, "Varna", "Zlatna Chaika", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(3, "Burgas", "Burgaska Batka", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(4, "Burgas", "Burgas", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(5, "Plovdiv", "NovotelTwo", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(6, "Sofia", "Komunist Hotel", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(7, "Plovdiv", "NovotelThree", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(8, "Varna", "Grand Hotel Varna", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(10, "Plovdiv", "Prime Holding", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(9, "Sofia", "Rodina", 4, 3, "150 lv. per night"));
            hotels.Add(new Hotels(11, "Plovdiv", "Maina", 4, 3, "150 lv. per night"));
        }

        public HandleUpdateService(ITelegramBotClient _botClient)
        {
            botClient = _botClient;
            FillHotels();
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            if (message.Text.Split(' ').First().ToLower().Equals("search") || searchHotel)
            {
                searchHotel = true;
                await SearchHotels(botClient, message);
            }
            else if (!searchHotel)
            {
                var action = (message.Text.Split(' ').First()) switch
                {
                    
                    _ => Usage(botClient, message)
                };

                var sentMessage = await action;
                Console.WriteLine($"The message was sent with id: {sentMessage.MessageId}");

            }
        

        static async Task SearchHotels(ITelegramBotClient botClient, Message message)
        {
            var id = message.Chat.Id;
            var text = message.Text;

            Console.WriteLine(text);

            if (!townChosen)
            {
                townChosen = true;
                await botClient.SendTextMessageAsync(id, "Enter the name of the city:");
            }
            else if (townChosen && !hotelChosen)
            {
                FillHotels();
                hotelChosen = true;
                string choice = "";
                int counter = 1;


                List<InlineKeyboardButton> buttonsOfHotels = new List<InlineKeyboardButton>();


                foreach (var hotel in hotels.Where(x => x.City.Equals(text)))
                {
                    buttonsOfHotels.Add(InlineKeyboardButton.WithCallbackData(hotel.HotelName, hotel.HotelName));
                    choice += counter + ". " + hotel.HotelName + "\n";
                    counter++;
                }

                //ID needed
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
           {
                    // first row 
                    new []
                    {
                        buttonsOfHotels[0],
                        buttonsOfHotels[1],
                        buttonsOfHotels[0],
                    },
                    // second row
                    new []
                    {
                        buttonsOfHotels[2],
                        buttonsOfHotels[3],
                        buttonsOfHotels[0],

                    },
                    new []
                    {
                        buttonsOfHotels[2],
                        buttonsOfHotels[3],
                        buttonsOfHotels[0],

                    },
                });

                //await botClient.SendTextMessageAsync(id, $"Please, choose a hotel:\n {choice}");
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Please choose a hotel:",
                                                            replyMarkup: inlineKeyboard);
            }
            else if (townChosen && hotelChosen)
            {
                string facts = "";
                Hotels myHotel = hotels.Where(x => x.HotelName.Equals(text)).ToList()[0];
                facts += $"Hotel rating: {myHotel.HotelRating} \n";
                facts += $"Guest Review Rating: {myHotel.GuestReviewRating} \n";
                facts += $"Price for a room: {myHotel.CurrentPrice} lv. \n";

                await botClient.SendTextMessageAsync(id, $"Facts about the hotel:\n {facts}");
                searchHotel = false;
            }

        }
        

            static async Task<Message> Usage(ITelegramBotClient bot, Message message)
            {
                const string usage = "Usage:\n" +
                                     "Type 'search' to begin searching for hotel in a particular city";

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: usage,
                                                      replyMarkup: new ReplyKeyboardRemove());
            }
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
         /*   await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}"); */

            Message message = new Message();
            message.Text = callbackQuery.Data;
            message.Chat = callbackQuery.Message.Chat;
            await BotOnMessageReceived(message);
        }

        #region Inline Mode

        private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
        {

            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                    results: results,
                                                    isPersonal: true,
                                                    cacheTime: 0);
        }

        private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
        {
            return Task.CompletedTask;
        }

        #endregion

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            return Task.CompletedTask;
        }

    }
}