using ProjectB.Clients.Models;
using ProjectB.Enums;
using ProjectB.Services;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class CheckInSelectState : IState
    {
        private ICosmosDbService<UserInformation> _cosmosDbService;

        public CheckInSelectState(ICosmosDbService<UserInformation> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var userInformation = new UserInformation();
            userInformation.Id = callbackQuery.Message.Chat.Id.ToString();
            userInformation.CheckInDate = callbackQuery.Data.ToString();
            await _cosmosDbService.AddCheckInDateAsync(userInformation);
            await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
            return State.CheckInSelectState;
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.CheckInSelectState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Show CheckOut Dates"),
                    }
            });

            await botClient.SendTextMessageAsync(chatId, "Select", replyMarkup: inlineKeyboard);
        }
    }
}
