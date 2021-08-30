using System.Text.Json.Serialization;

namespace ProjectB.Clients.Models.Hotels
{
    public class RatePlan
    {
        public Price Price { get; set; }

        [JsonIgnore]
        public object Features { get; set; }
    }
}
