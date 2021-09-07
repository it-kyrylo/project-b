using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models.HotelDetails
{
    public class HotelService
    {
        public string Heading { get; set; }

        [JsonPropertyName("listItems")]
        public string[] ServiceDescription { get; set; }
    }
}
