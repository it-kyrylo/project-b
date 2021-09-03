using AutoMapper;
using ProjectB.Clients;
using ProjectB.Clients.Models;
using ProjectB.Clients.Models.HotelDetails;
using ProjectB.Clients.Models.Hotels;
using ProjectB.Infrastructure;
using ProjectB.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ProjectB.Services
{
    public class HotelService : IHotelService
    {
        private IHotelClients hotelClients;
        private IMapper mapper;
        private CacheFilter cacheFilter;
        private readonly ICacheFilter<HotelOverview> _hotelOverviewCache;

        public HotelService(IHotelClients hotelClients, IMapper mapper, CacheFilter cacheFilter, ICacheFilter<HotelOverview> hotelOverviewCache)
        {
            this.hotelClients = hotelClients;
            this.mapper = mapper;
            this.cacheFilter = cacheFilter;
            _hotelOverviewCache = hotelOverviewCache;
        }

        public async Task<int> GetDestinationIdAsync(string cityName)
        {
            string formattedCityName = string.Join(',', cityName.Split(new char[] {' ',','}, StringSplitOptions.RemoveEmptyEntries)).ToUpperInvariant();

            var cacheKey = formattedCityName;
            var cacheValue = cacheFilter.GetCache(cacheKey);
            var destinationId = 0;

            if (cacheValue != null)
            {
                var cachedDestination = (LocationsByCity)cacheValue;

                foreach (var item in cachedDestination.Suggestions)
                {
                    foreach (var number in item.CityProperties)
                    {
                        destinationId = int.Parse(number.DestinationId);
                        break;
                    }
                    break;
                }
                return destinationId;
            }
            var destinationToCache = await this.hotelClients.GetDestination(cityName);
            int cacheAbsoluteExpiration = 6;
            int cacheSlidingExpiration = 3;
            await cacheFilter.SetCache(cacheKey, destinationToCache, cacheAbsoluteExpiration, cacheSlidingExpiration);
            return await GetDestinationIdAsync(cityName);
        }

        public async Task<ICollection<HotelsViewModel>> GetHotelsByDestinationIdAsync(int id)
        {
            var cacheKey = id.ToString();
            var cacheValue = cacheFilter.GetCache(cacheKey);
            var hotelsViewModel = new List<HotelsViewModel>();

            if (cacheValue != null)
            {
                try
                {
                    var cachedHotels = (HotelsLocationByCity)cacheValue;
                    foreach (var item in cachedHotels.Data.Body.SearchResults.Results)
                    {
                        var hotel = new HotelsViewModel();
                        hotel = this.mapper.Map<HotelByCity, HotelsViewModel>(item);
                        hotelsViewModel.Add(hotel);
                    }
                return hotelsViewModel;
                }
                catch (Exception)
                {
                    throw new ("ID Not Found!");
                }
                
            }
            var hotelsToCache = await this.hotelClients.GetHotels(id);
            await cacheFilter.SetCache(cacheKey, hotelsToCache);
            return await GetHotelsByDestinationIdAsync(id);
        }

        public async Task<HotelOverview> GetHotelDetailsById(int id, string checkIn, string checkOut)
        {
            var cacheKey = $"{id}_{checkIn}_{checkOut}";
            var hotelDetails = _hotelOverviewCache.Get(cacheKey);

            if (hotelDetails == null)
            {
                hotelDetails = await hotelClients.GetHotel(id, checkIn, checkOut);
                _hotelOverviewCache.Set(cacheKey, hotelDetails);
            }

            return hotelDetails;
        }
    }
}
