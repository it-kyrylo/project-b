namespace ProjectB.Clients.Models;

public class HotelCache
{
    public HotelCache(long chatId, string destinationId)
    {
        ChatId = chatId;
        DestinationId = destinationId;
    }

    public long ChatId { get; set; }

    public string DestinationId { get; set; }
}
