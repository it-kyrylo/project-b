using ProjectB.Enums;
using ProjectB.Factories;
using ProjectB.Services;
using ProjectB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProjectB.Handlers
{
    public class TelegramUpdateHandler : ITelegramUpdateHandler
    {
        private IStateFactory _statefactory;
        private int _count;

        public TelegramUpdateHandler(IStateFactory statefactory)
        {
            _statefactory = statefactory;
            _count = 0;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message == null && update.CallbackQuery == null)
            {
                return;
            }

            var chatId = update.Message != null ? update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;
            var states = Enum.GetValues<State>();
            var currentState = states[_count];
            var state = _statefactory.GetState(currentState);

            var handler = update.Type switch
            {
                UpdateType.Message => state.BotOnMessageReceived(botClient, update.Message).Result,
                UpdateType.CallbackQuery => state.BotOnCallBackQueryReceived(botClient, update.CallbackQuery).Result,
                _ => UnknownUpdateHandlerAsync(botClient, update).Result
            };

            try
            {
                _count++;
                if (_count < states.Length)
                {
                    currentState = states[_count];
                }
                else
                {
                    _count = 0;
                }
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken).ConfigureAwait(false);
            }

            //if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            //{
            //    if (update.Message.Text.ToString().ToLower() == "/start")
            //    {
            //        var states = _statefactory.GetState(State.MainState);
            //        await states.BotOnMessageReceived(botClient, update.Message);
            //        //await botClient.SendTextMessageAsync(chatId, test.Text, replyMarkup: test.ReplyMarkup);
            //    }
            //    else if (states.ContainsKey(chatId))
            //    {
            //        //var hotels = await this._messageBuilder.HotelsToButtons(update.Message.Text.ToString());
            //        //var hotelstates = (Message)_statefactory[State.CityTypedFromUSer];
            //        //await botClient.SendTextMessageAsync(chatId, hotelstates.Text, replyMarkup: hotelstates.ReplyMarkup);
            //    }

            //}
            //else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            //{
            //    var state = string.Empty;
            //    states.TryGetValue(chatId, out state);
            //    if (state == null)
            //    {
            //        this.states.Add(chatId, update.CallbackQuery.Data.ToString());
            //        await botClient.SendTextMessageAsync(chatId, update.CallbackQuery.Data.ToString());
            //    }
            //    else
            //    {
            //        if (!this.Date.ContainsKey(chatId))
            //        {
            //            this.Date.Add(chatId, new SaveData());
            //        }
            //        else if (this.Date[chatId].HotelId != 0 && this.Date[chatId].CheckIn != null && this.Date[chatId].CheckOut == null)
            //        {
            //            this.Date[chatId].CheckOut = update.CallbackQuery.Data.ToString();
            //            cout = 0;
            //            //var hotel = await _messageBuilder.HotelInfo
            //                //(this.Date[chatId].HotelId, this.Date[chatId].CheckIn, this.Date[chatId].CheckOut);
            //            //await botClient.SendTextMessageAsync(chatId, hotel.Text, replyMarkup: hotel.ReplyMarkup);
            //            this.Date = new Dictionary<long, SaveData>();
            //            this.states = new Dictionary<long, string>();
            //            return;
            //        }
            //        if (cout % 2 == 0)
            //        {
            //            //var checkin = this._messageBuilder.CheckInDate();
            //            //await botClient.SendTextMessageAsync(chatId, checkin.Text, replyMarkup: checkin.ReplyMarkup);
            //            this.Date[chatId].HotelId = int.Parse(update.CallbackQuery.Data.ToString());
            //            cout++;
            //        }
            //        else if (cout % 2 != 0)
            //        {
            //            this.Date[chatId].CheckIn = update.CallbackQuery.Data.ToString();
            //            //var chekout = this._messageBuilder.CheckOutDate();
            //            //await botClient.SendTextMessageAsync(chatId, chekout.Text, replyMarkup: chekout.ReplyMarkup);
            //            cout++;
            //        }
            //    }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);

            return Task.CompletedTask;
        }

        private Task<State> UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(update.Message.Chat.Id, "Something went wrong! Please try again");

            return Task.Run(() => State.MainState);
        }
}
}
