using System.Text.Json.Serialization;

namespace ProjectB.Infrastructure
{
    public class CosmosDbUserHistory
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("searchHistory")]
        public string SearchHistory { get; set; }
    }
}
