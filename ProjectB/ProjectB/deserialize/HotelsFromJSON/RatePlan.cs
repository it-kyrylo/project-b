using System.Text.Json.Serialization;

namespace ProjectB.deserialize.HotelsFromJSON
{
    public class RatePlan
    {
        public Price Price { get; set; }

        [JsonIgnore]
        public object Features { get; set; }
    }
}
