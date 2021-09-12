using ProjectB.Models.States.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectB.States
{
    public interface IState
    {
        Task<ContextState> BotOnMessageReceived(ITelegramBotClient botClient, Message message);

        Task<ContextState> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        Task BotSendMessage(ITelegramBotClient botClient, long chatId);
    }
}
