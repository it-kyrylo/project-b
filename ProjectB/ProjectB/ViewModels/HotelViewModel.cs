namespace ProjectB.ViewModels;

public class HotelViewModel
{
    public string Name { get; set; }

    public string PostalCode { get; set; }

    public string Address { get; set; }

    public double StarRating { get; set; }

    public string StaticMapLink { get; set; }

    public decimal Price { get; set; }

    public string BookingLink = "www.hotels.com/hoid/?q-check-in=checkindate&q-check-out=checkoutdate";

    public string[] HotelService { get; set; }
}
