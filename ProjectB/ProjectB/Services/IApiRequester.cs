using Refit;
using ProjectB.deserialize;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectB.deserialize.HotelsFromJSON;
using ProjectB.deserialize.HotelDetailsFromJSON;

namespace ProjectB.Services
{
    public interface IApiRequester
    {
        
        [Get("/locations/search?query={CityName}&locale=en_US")]
        public Task<LocationsByCity> Destination(string CityName);

        [Get("/properties/list?destinationId={id}")]
        public Task<HotelsLocationByCity> Hotels(int id);

        [Get("/properties/get-details?id={id}&checkIn={checkIn}&checkOut={checkOut}")]
        public Task<HotelOverview> Hotel(int id, string checkIn, string checkOut);
    }
}
