namespace ProjectB.States;

public class HotelInfoState : IState
{
    private IHotelService _hotelService;
    private ICosmosDbService<UserInformation> _cosmosDbService;

    public HotelInfoState(IHotelService hotelService, ICosmosDbService<UserInformation> cosmosDbService)
    {
        _hotelService = hotelService;
        _cosmosDbService = cosmosDbService;
    }

    public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
        return State.HotelInfoState;
    }

    public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    => Task.FromResult(State.HotelInfoState);

    public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
    {
        var hotelId = await _cosmosDbService.GetHotelIdByChatIdAsync(chatId.ToString());
        var checkIn = await _cosmosDbService.GetCheckInDateByChatIdAsync(chatId.ToString());
        var checkOut = await _cosmosDbService.GetCheckOutDateByChatIdAsync(chatId.ToString());
        
        var hotel = await _hotelService.GetHotelDetailsById(int.Parse(hotelId), checkIn, checkOut);
        hotel.BookingLink = hotel.BookingLink.Replace("id", hotelId.ToString());
        hotel.BookingLink = hotel.BookingLink.Replace("checkindate", checkIn);
        hotel.BookingLink = hotel.BookingLink.Replace("checkoutdate", checkOut);
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData($"Name: {hotel.Name}",hotel.Name),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData($"PostalCode: {hotel.PostalCode}",hotel.PostalCode),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"Price: {hotel.Price.ToString()}",hotel.Price.ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"Address: {hotel.Address}",hotel.Address),
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("Map",hotel.StaticMapLink),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"StarRating: {hotel.StarRating.ToString()}",hotel.StarRating.ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("book",hotel.BookingLink),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Back","Back"),
                },
        });

        var message = new Message();
        message.Text = "This is The Hotel Info";
        message.ReplyMarkup = inlineKeyboard;

        await botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: inlineKeyboard);
    }
}
