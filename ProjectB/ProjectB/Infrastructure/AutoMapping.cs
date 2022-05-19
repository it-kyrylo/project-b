namespace ProjectB.Infrastructure;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<HotelByCity, HotelsViewModel>()
            .ForMember(x => x.Address, opt => opt.MapFrom(x => x.Address.StreetAddress))
            .ForMember(x => x.GuestReviewRating, opt => opt.MapFrom(x => x.GuestReview.Rating))
            .ForMember(x => x.CurrentPrice, opt => opt.MapFrom(x => x.RatePlan.Price.CurrentPrice));
        CreateMap<Hotel, HotelViewModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.HotelDescription.Name))
            .ForMember(x => x.PostalCode, opt => opt.MapFrom(x => x.HotelDescription.Address.PostalCode))
            .ForMember(x => x.Address, opt => opt.MapFrom(x => x.HotelDescription.Address.FullAddress))
            .ForMember(x => x.StarRating, opt => opt.MapFrom(x => x.HotelDescription.StarRating))
            .ForMember(x => x.StaticMapLink, opt => opt.MapFrom(x => x.HotelDescription.MapWidget.StaticMapUrl))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.HotelDescription.FeaturedPrice.currentPrice.Plain));
            
    }
}
