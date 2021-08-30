using ProjectB.Clients.Models.HotelDetails;
using ProjectB.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface IHotelService
    {
        public Task<int> GetDestinationIdAsync(string cityName);

        public Task<ICollection<HotelsViewModel>> GetHotelsByDestinationIdAsync(int id);

        public Task<HotelOverview> GetHotelDetailsById(int id, string checkIn, string checkOut);
    }
}
