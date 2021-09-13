using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ProjectB.Services
{
    public interface IMessageBuilder
    {

        Message MainState(ChatId chatId);

        Task<Message> HotelsToButtons(ChatId chatId, string cityName);

        Message CheckInDate();

        Message CheckOutDate();

        Task<Message> HotelInfo(int hotelId,string checkIn, string checkOut);
    }
}
