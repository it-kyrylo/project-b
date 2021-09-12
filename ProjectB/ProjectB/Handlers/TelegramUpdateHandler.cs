using ProjectB.Factories;
using ProjectB.Models.States.Enums;
using ProjectB.Services;
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
        // private readonly IStateProviderService _stateProvider;
        private readonly IStateFactory _stateFactory;
        public TelegramUpdateHandler(IStateFactory stateFactory)
        {
            //_stateProvider = stateProvider;
            _stateFactory = stateFactory;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message == null && update.CallbackQuery == null)
            {
                return;
            }

            var chatId = update.Message != null ? update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;
            var state = _stateFactory.GetState(ContextState.MainState);

            var handler = update.Type switch
            {
                UpdateType.Message => state.BotOnMessageReceived(botClient, update.Message).Result,
                UpdateType.CallbackQuery => state.BotOnCallBackQueryReceived(botClient, update.CallbackQuery).Result,
                _ => UnknownUpdateHandlerAsync(botClient, update).Result
            };

            try
            {
                var nextState = handler;
               // var chat = await _stateProvider.GetChatStateAsync(chatId);
               // chat.Current_State = nextState;
              //  await _stateProvider.UpdateChatStateAsync(chat);
                await _stateFactory.GetState(nextState).BotSendMessage(botClient, chatId);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken).ConfigureAwait(false);
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

        private Task<ContextState> UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(update.Message.Chat.Id, "Something went wrong! Please try again");

            return Task.Run(() => ContextState.MainState);
        }
    }
}
