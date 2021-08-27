using System.Text.Json.Serialization;

namespace ProjectB.deserialize.HotelsFromJSON
{
    public class GuestReview
    {
        [JsonIgnore]
        public int UnformattedRating { get; set; }

        public int Rating { get; set; }

        public int Total { get; set; }

        [JsonIgnore]
        public int Scale { get; set; }
    }
}
