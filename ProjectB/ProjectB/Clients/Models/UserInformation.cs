using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models
{
    public class UserInformation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("searchHistory")]
        public List<string> SearchHistory { get; set; }
        [JsonPropertyName("hotelId")]
        public string HotelId { get; set; }
        [JsonPropertyName("checkInDate")]
        public string CheckInDate { get; set; }
        [JsonPropertyName("checkOutDate")]
        public string CheckOutDate { get; set; }
    }
}
