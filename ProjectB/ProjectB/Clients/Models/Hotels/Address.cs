namespace ProjectB.Clients.Models.Hotels;

public class Address
{
    public string StreetAddress { get; set; }

    [JsonIgnore]
    public string ExtendedAddress { get; set; }

    [JsonPropertyName("locality")]
    public string City { get; set; }

    public string PostalCode { get; set; }

    public string Region { get; set; }

    public string CountryName { get; set; }

    public string CountryCode { get; set; }

    [JsonIgnore]
    public bool Obfuscate { get; set; }
}
