namespace ProjectB.Clients.Models.HotelDetails;

public class Amenities
{
    public string Heading { get; set; }

    [JsonPropertyName("listItems")]
    public HotelServices[] HotelService { get; set; }
}
