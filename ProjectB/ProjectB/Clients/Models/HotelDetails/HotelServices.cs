namespace ProjectB.Clients.Models.HotelDetails
{
    public class HotelServices
    {
        public string Heading { get; set; }

        [JsonPropertyName("listItems")]
        public string[] ServiceDescription { get; set; }
    }
}
