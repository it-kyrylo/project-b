namespace ProjectB.Clients.Models.HotelDetails;

public class HotelOverview 
{
    public string Result { get; set; }

    [JsonPropertyName("data")]
    public HotelDetails HotelDetails { get; set; }

    [JsonIgnore]
    [JsonPropertyName("transportation")]
    public object Transportation { get; set; }

    [JsonIgnore]
    public object Neighborhood { get; set; }
}
