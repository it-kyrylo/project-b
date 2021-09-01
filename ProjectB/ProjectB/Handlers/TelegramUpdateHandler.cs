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

        //private readonly HandleUpdateService handleUpdateServise;

        //public TelegramUpdateHandler(HandleUpdateService handleUpdateServise)
        //{
        //    this.handleUpdateServise = handleUpdateServise;
        //}

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message == null && update.CallbackQuery == null)
            {
                return;
            }

            var chatId = update.Message != null ? update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;
            var me = await botClient.GetMeAsync();
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                await botClient.SendTextMessageAsync(chatId, "running");
            }
            

            //try
            //{
            //    var nextState = await handler;
            //    var chat = _stateProvider.GetChatState(chatId);
            //    chat.State = nextState;
            //    _stateProvider.SaveChatState(chat);
            //    _stateFactory.GetState(nextState).BotSendMessage(botClient, chatId);
            //}
            //catch (Exception exception)
            //{
            //    await HandleErrorAsync(botClient, exception, cancellationToken).ConfigureAwait(false);
            //}
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
    }
}
