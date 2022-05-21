using Telegram.Bot.Exceptions;
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

            var chatId = GetChatId(update);
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
            else if (currentState != State.MainState)
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
                else if (_count + 1 == states.Length)
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

        private async void HandleCommunication(ITelegramBotClient botClient, Update update, States.IState state)
        {

            try
            {
                var handler = update.Type switch
                {
                    UpdateType.Message => await state.BotOnMessageReceived(botClient, update.Message),
                    UpdateType.CallbackQuery => await state.BotOnCallBackQueryReceived(botClient, update.CallbackQuery),
                    _ => await UnknownUpdateHandlerAsync(botClient, update)
                };
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(GetChatId(update), ex.Message);
                RepeatState(ex, botClient, update);
            }
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

        private async Task<State> UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Something went wrong! Please try again");

            return await Task.Run(() => State.MainState);
        }

        private long GetChatId(Update update)
        {
            return update.Message != null ? update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;
        }

        private void RepeatState(Exception ex, ITelegramBotClient botClient, Update update)
        {
            if (ex.StackTrace.Contains("GetDestinationIdAsync"))
            {
                _count = (int)State.CityTypedFromUserState;
                HandleCommunication(botClient, update, _statefactory.GetState(State.CitySelectState));
            }
            else if (ex.StackTrace.Contains("HotelInfoState"))
            {
                _count = (int)State.HotelSelectState;
                HandleCommunication(botClient, update, _statefactory.GetState(State.CityTypedFromUserState));
            }

        }
    }
}
