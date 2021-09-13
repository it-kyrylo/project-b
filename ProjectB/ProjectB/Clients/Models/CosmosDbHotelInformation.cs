using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectB.Clients.Models
{
    public class CosmosDbHotelInformation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("hotelId")]
        public string HotelId { get; set; }
        [JsonPropertyName("checkInDate")]
        public string CheckInDate { get; set; }
        [JsonPropertyName("checkOutDate")]
        public string CheckOutDate { get; set; }
    }
}
