using ProjectB.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class CheckOutSelectState : IState
    {
        public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var chechOutDate = callbackQuery.Data.ToString();
            await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
            return State.CheckOutSelectState;
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.CheckOutSelectState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Show HotelInfo"),
                    }
            });

            await botClient.SendTextMessageAsync(chatId, "Select", replyMarkup: inlineKeyboard);
        }
    }
}
