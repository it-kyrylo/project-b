using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ProjectB.deserialize.HotelsFromJSON
{
    public class SearchResult
    {
        [JsonIgnore]
        public string totalCount { get; set; }

        public ICollection<HotelByCity> Results { get; set; }

        [JsonIgnore]
        public string Pagination { get; set; }
    }
}
