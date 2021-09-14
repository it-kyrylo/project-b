using ProjectB.Clients.Models;
using ProjectB.Enums;
using ProjectB.Services;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectB.States
{
    public class CheckOutSelectState : IState
    {
        private ICosmosDbService<UserInformation> _cosmosDbService;

        public CheckOutSelectState(ICosmosDbService<UserInformation> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var userInformation = new UserInformation();
            userInformation.Id = callbackQuery.Message.Chat.Id.ToString();
            userInformation.CheckOutDate = callbackQuery.Data.ToString();
            await _cosmosDbService.AddCheckOutDateAsync(userInformation);
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
