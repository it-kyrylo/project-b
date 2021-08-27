
using AutoMapper;
using ProjectB.deserialize;
using ProjectB.deserialize.HotelDetailsFromJSON;
using ProjectB.deserialize.HotelsFromJSON;
using ProjectB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public class HotelService : IHotelService
    {
        private IApiRequester apiRequester;
        private IMapper mapper;

        public HotelService(IApiRequester apiRequester, IMapper mapper)
        {
            this.apiRequester = apiRequester;
            this.mapper = mapper;
        }

        public async Task<int> GetDestinationIdAsync(string cityName)
        {
            var destination = await this.apiRequester.Destination(cityName);


            var destinationId = 0;
            foreach (var item in destination.Suggestions)
            {
                foreach (var number in item.Entities)
                {
                    destinationId = int.Parse(number.DestinationId);
                    break;
                }
                break;
            }

            return destinationId;
        }

        public async Task<ICollection<HotelsViewModel>> GetHotelsByDestinationIdAsync(int id)
        {
            var hotels = await this.apiRequester.Hotels(id);
            var hotelsViewModel = new List<HotelsViewModel>();

            foreach (var item in hotels.Data.Body.SearchResults.Results)
            {
                var hotel = new HotelsViewModel();
                hotel = this.mapper.Map<HotelByCity, HotelsViewModel>(item);
                hotelsViewModel.Add(hotel);
            }

            return hotelsViewModel;
        }

        public async Task<HotelOverview> GetHotelDetailsById(int id, string checkIn, string checkOut)
        {
            var hotelDetails = await this.apiRequester.Hotel(id, checkIn, checkOut);
            
            return hotelDetails;
        }

    }
}
