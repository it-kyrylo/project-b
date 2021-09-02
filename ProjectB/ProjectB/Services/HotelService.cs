using AutoMapper;
using ProjectB.Clients;
using ProjectB.Clients.Models;
using ProjectB.Clients.Models.HotelDetails;
using ProjectB.Clients.Models.Hotels;
using ProjectB.ViewModels;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public class HotelService : IHotelService
    {
        private IHotelClients hotelClients;
        private IMapper mapper;

        public HotelService(IHotelClients hotelClients, IMapper mapper)
        {
            this.hotelClients = hotelClients;
            this.mapper = mapper;
        }

        public async Task<ICollection<HotelsViewModel>> GetDestinationIdAsync(string cityName)
        {
            var destination = await this.hotelClients.GetDestination(cityName);


            var destinationId = 0;
            foreach (var item in destination.Suggestions)
            {
                foreach (var number in item.CityProperties)
                {
                    destinationId = int.Parse(number.DestinationId);
                    break;
                }
                break;
            }

            return await GetHotelsByDestinationIdAsync(destinationId);
        }

        public async Task<ICollection<HotelsViewModel>> GetHotelsByDestinationIdAsync(int id)
        {
            var hotels = await this.hotelClients.GetHotels(id);
            var hotelsViewModel = new List<HotelsViewModel>();

            foreach (var item in hotels.Data.Body.SearchResults.Results)
            {
                var hotel = new HotelsViewModel();
                hotel = this.mapper.Map<HotelByCity, HotelsViewModel>(item);
                hotelsViewModel.Add(hotel);
            }

            return hotelsViewModel;
        }

        public async Task<HotelViewModel> GetHotelDetailsById(int id, string checkIn, string checkOut)
        {
            var hotelDetails = await this.hotelClients.GetHotel(id, checkIn, checkOut);
            var Hotel = new HotelViewModel();
            Hotel = this.mapper.Map(hotelDetails.HotelDetails.Hotel, Hotel);
            var services = hotelDetails.HotelDetails.Hotel.Amenities.Where(x => x.Heading == "In the hotel")
                .SelectMany(x => x.HotelService.Where(x => x.Heading == "Services")).ToArray();
            foreach (var item in services)
            {
                Hotel.HotelService = item.ServiceDescription;
            }
            return Hotel;
        }

    }
}
