using ProjectB.Clients.Models.Hotels;
using System.Collections.Generic;

namespace ProjectB.ViewModels
{
    public class HotelsViewModel
    {
        public int Id { get; set; }

        public string HotelName { get; set; }

        public double HotelRating { get; set; }

        public string Address { get; set; }

        public int GuestReviewRating { get; set; }

        public ICollection<LandMark> LandMarks { get; set; }

        public string CurrentPrice { get; set; }
    }
}
