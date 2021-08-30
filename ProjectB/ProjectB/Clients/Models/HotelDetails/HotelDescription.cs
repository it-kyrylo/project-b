using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models.HotelDetails
{
    public class HotelDescription
    {
        [JsonIgnore]
        public string ClientToken { get; set; }

        public Address Address { get; set; }

        public bool PriceMatchEnabled { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public string StarRatingTitle { get; set; }

        public double StarRating { get; set; }

        public FeaturedPrice FeaturedPrice { get; set; }

        public MapWidget MapWidget { get; set; }

        public string[] RoomTypeNames { get; set; }

        [JsonIgnore]
        public string[] Tagline { get; set; }

        [JsonIgnore]
        public string[] FreeBies { get; set; }
    }
}
