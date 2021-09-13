using ProjectB.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectB.States
{
    public interface IState
    {
        Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message);

        Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        Task BotSendMessage(ITelegramBotClient botClient, long chatId);
    }
}
