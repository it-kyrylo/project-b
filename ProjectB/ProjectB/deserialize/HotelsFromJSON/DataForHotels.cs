using System.Text.Json.Serialization;

namespace ProjectB.deserialize.HotelsFromJSON
{
    public class DataForHotels
    {
        public Body Body { get; set; }

        [JsonIgnore]
        public string Common { get; set; }
    }
}
