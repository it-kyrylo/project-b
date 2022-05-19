namespace ProjectB.Clients.Models.Hotels;

public class SearchResult
{
    [JsonIgnore]
    public string totalCount { get; set; }

    public ICollection<HotelByCity> Results { get; set; }

    [JsonIgnore]
    public string Pagination { get; set; }
}
