using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models.HotelDetails
{
    public class HotelDetails
    {
        [JsonPropertyName("body")]
        public Hotel Hotel { get; set; }

        [JsonIgnore]
        public object Common { get; set; }
    }
}
