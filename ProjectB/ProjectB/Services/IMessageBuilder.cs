using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ProjectB.Services
{
    public interface IMessageBuilder
    {
        Message MainState();

        Task<Message> HotelsToButtons(string cityName);

        Message CheckInDate();

        Message CheckOutDate();

        Task<Message> HotelInfo(int hotelId,string checkIn, string checkOut);
    }
}
