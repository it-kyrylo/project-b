﻿using ProjectB.Clients.Models.HotelDetails;
using ProjectB.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface IHotelService
    {
        public Task<ICollection<HotelsViewModel>> GetDestinationIdAsync(string cityName);

        public Task<ICollection<HotelsViewModel>> GetHotelsByDestinationIdAsync(int id);

        public Task<HotelViewModel> GetHotelDetailsById(int id, string checkIn, string checkOut);
    }
}
