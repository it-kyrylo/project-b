namespace ProjectB.Clients.Models.Hotels
{
    public class DataForHotels
    {
        public Body Body { get; set; }

        [JsonIgnore]
        public string Common { get; set; }
    }
}
