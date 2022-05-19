namespace ProjectB.Clients.Models.HotelDetails;

public class Hotel
{
    [JsonIgnore]
    public object PdpHeader { get; set; }

    [JsonIgnore]
    public object OverView { get; set; }

    [JsonIgnore]
    public object HotelWelcomeRewards { get; set; }

    [JsonPropertyName("propertyDescription")]
    public HotelDescription HotelDescription { get; set; }

    public GuestReviews GuestReviews { get; set; }

    [JsonIgnore]
    public object AtAGlance { get; set; }

    public Amenities[] Amenities { get; set; }

    [JsonIgnore]
    public object HygieneAndCleanliness { get; set; }

    [JsonIgnore]
    public object SmallPrint { get; set; }

    [JsonIgnore]
    public object SpecialFeatures { get; set; }

    [JsonIgnore]
    public object Miscellaneous { get; set; }

    [JsonIgnore]
    public object PageInfo { get; set; }

    [JsonIgnore]
    public object Unavailable { get; set; }
}
