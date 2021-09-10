using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectB.Infrastructure
{
    public class CosmosDbSearchHistory
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("searchHistory")]
        public string SearchHistory { get; set; }
    }
}
