using System.Text.Json.Serialization;

namespace ProjectB.deserialize.HotelsFromJSON
{
    public class Price
    {
        [JsonPropertyName("current")]
        public string CurrentPrice { get; set; }

        [JsonPropertyName("exactCurrent")]
        public decimal ExactPrice { get; set; }
    }
}
