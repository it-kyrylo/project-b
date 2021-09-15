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
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                if (update.Message.Text.ToString().ToLower() == "/start")
                {
                    HandleCommunication(botClient, update, state);
                }
                else if (currentState == State.CityTypedFromUserState)
                {
                    HandleCommunication(botClient, update, state);
                }
            }
            else if (update.CallbackQuery.Data.ToString() == "Help")
            {
                state = _statefactory.GetState(State.HelpState);
                HandleCommunication(botClient, update, state);
                _count = 0;
            }
            else if (update.CallbackQuery.Data.ToString() == "Back")
            {
                state = _statefactory.GetState(State.MainState);
                HandleCommunication(botClient, update, state);
                _count = 0;
            }
            else if(currentState != State.MainState)
            {
                HandleCommunication(botClient, update, state);
            }
            else if (currentState == State.HelpState)
            {
                HandleCommunication(botClient, update, state);
                return;
            }
            else
            {
                currentState = State.MainState;
                return;
            }

            try
            {
                _count++;
                if (_count < states.Length)
                {
                    currentState = states[_count];
                }
                else if(_count + 1 == states.Length)
                {
                    _count = 0;
                    currentState = states[_count];
                }
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken).ConfigureAwait(false);
            }
        }

        private void HandleCommunication(ITelegramBotClient botClient, Update update, States.IState state)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => state.BotOnMessageReceived(botClient, update.Message).Result,
                UpdateType.CallbackQuery => state.BotOnCallBackQueryReceived(botClient, update.CallbackQuery).Result,
                _ => UnknownUpdateHandlerAsync(botClient, update).Result
            };
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
