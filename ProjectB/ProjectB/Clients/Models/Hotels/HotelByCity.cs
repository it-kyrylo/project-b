namespace ProjectB.Clients.Models.Hotels;

public class HotelByCity
{
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string HotelName { get; set; }

    [JsonPropertyName("starRating")]
    public double HotelRating { get; set; }

    [JsonIgnore]
    public string urls { get; set; }

    public Address Address { get; set; }

    public GuestReview GuestReview { get; set; }

    public ICollection<LandMark> LandMarks { get; set; }

    [JsonIgnore]
    public string GeoBullets { get; set; }

    public RatePlan RatePlan { get; set; }

    [JsonIgnore]
    public string Neighbourhood { get; set; }

    [JsonPropertyName("coordinate")]
    public Coordinate Coordinates { get; set; }

    [JsonIgnore]
    public string ProviderType { get; set; }

    [JsonIgnore]
    public int SupplierHotelId { get; set; }

    [JsonIgnore]
    public string VrBadge { get; set; }

    [JsonIgnore]
    public bool IsAlternative { get; set; }

    [JsonIgnore]
    public string OptimizedThumbUrls { get; set; }
}
