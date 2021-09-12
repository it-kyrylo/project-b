using ProjectB.Models.States.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProjectB.States
{
    public class MainState : IState
    {
        public async Task<ContextState> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return ContextState.MainState;
            }

            return message.Text switch
            {
                //TODO: Add players Statitics and Teams Statistics menue states
                //TODO: Introduce StatesConstants class
                _ => await PrintMessage(botClient, message.Chat.Id, "Just a test -> MessageBuilderHere", ContextState.MainState)
            };
        }

        public Task<ContextState> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            => Task.FromResult(ContextState.MainState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            //TODO: Introduce StatesConstants class
            await botClient.SendTextMessageAsync(chatId, "MessageGoesHere");
        }

        private static async Task<ContextState> PrintMessage(ITelegramBotClient botClient, long chatId, string message, ContextState returnState)
        {
            await botClient.SendTextMessageAsync(chatId, message);

            return returnState;
        }
    }
}
