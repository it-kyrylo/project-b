namespace ProjectB.Clients.Models.HotelDetails;

public class FeaturedPrice
{
    public string BeforePriceText { get; set; }

    public string AfterPriceText { get; set; }

    public string PricingAvailability { get; set; }

    public string PricingTooltip { get; set; }

    public CurrentPrice currentPrice { get; set; }
}
