using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB.TelegramBot
{
    public class Hotels
    {
        public int ID { get; set; }

        public string City { get; set; }

        public string HotelName { get; set; }

        public double HotelRating { get; set; }

        public int GuestReviewRating { get; set; }



        // public ICollection<string> Landmarks { get; set; }

        public string CurrentPrice { get; set; }
        public Hotels(int id, string city, string name, double rating, int guestReviewRating, string price)
        {
            this.ID = id;
            this.City = city;
            this.HotelName = name;
            this.HotelRating = rating;
            this.GuestReviewRating = guestReviewRating;
            this.CurrentPrice = price;
        }
    }
}
