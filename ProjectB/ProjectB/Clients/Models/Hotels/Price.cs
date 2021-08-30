using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models.Hotels
{
    public class Price
    {
        [JsonPropertyName("current")]
        public string CurrentPrice { get; set; }

        [JsonPropertyName("exactCurrent")]
        public decimal ExactPrice { get; set; }
    }
}
