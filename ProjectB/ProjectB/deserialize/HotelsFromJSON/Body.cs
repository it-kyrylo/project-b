using System.Text.Json.Serialization;

namespace ProjectB.deserialize.HotelsFromJSON
{
    public class Body
    {
        [JsonIgnore]
        public string Header { get; set; }

        [JsonIgnore]
        public string Query { get; set; }

        public SearchResult SearchResults { get; set; }

        [JsonIgnore]
        public string SortResults { get; set; }

        [JsonIgnore]
        public string Filters { get; set; }

        [JsonIgnore]
        public string PointOfSale { get; set; }

        [JsonIgnore]
        public string Miscellaneous { get; set; }

        [JsonIgnore]
        public string PageInfo { get; set; }
    }
}
