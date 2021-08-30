using Refit;
using ProjectB.Clients.Models;
using ProjectB.Clients.Models.Hotels;
using ProjectB.Clients.Models.HotelDetails;

using System.Threading.Tasks;

namespace ProjectB.Clients
{
    public interface IHotelClients
    {
        
        [Get("/locations/search?query={CityName}&locale=en_US")]
        public Task<LocationsByCity> GetDestination(string CityName);

        [Get("/properties/list?destinationId={id}")]
        public Task<HotelsLocationByCity> GetHotels(int id);

        [Get("/properties/get-details?id={id}&checkIn={checkIn}&checkOut={checkOut}")]
        public Task<HotelOverview> GetHotel(int id, string checkIn, string checkOut);
    }
}
