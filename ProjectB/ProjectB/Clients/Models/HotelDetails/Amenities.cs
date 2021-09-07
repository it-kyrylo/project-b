using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models.HotelDetails
{
    public class Amenities
    {
        public string Heading { get; set; }

        [JsonPropertyName("listItems")]
        public HotelService[] HotelService { get; set; }
    }
}
