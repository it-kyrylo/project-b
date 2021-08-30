﻿using AutoMapper;
using ProjectB.Clients.Models.Hotels;
using ProjectB.ViewModels;

namespace ProjectB.Infrastructure
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<HotelByCity, HotelsViewModel>()
                .ForMember(x => x.Address, opt => opt.MapFrom(x => x.Address.StreetAddress))
                .ForMember(x => x.GuestReviewRating, opt => opt.MapFrom(x => x.GuestReview.Rating))
                .ForMember(x => x.CurrentPrice, opt => opt.MapFrom(x => x.RatePlan.Price.CurrentPrice));
        }
    }
}
