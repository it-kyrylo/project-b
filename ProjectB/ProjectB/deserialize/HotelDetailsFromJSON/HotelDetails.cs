using System.Text.Json.Serialization;

namespace ProjectB.deserialize.HotelDetailsFromJSON
{
    public class HotelDetails
    {
        [JsonPropertyName("body")]
        public Hotel Hotel { get; set; }

        [JsonIgnore]
        public object Common { get; set; }
    }
}
