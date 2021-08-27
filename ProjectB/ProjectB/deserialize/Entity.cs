using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.deserialize
{
    public class Entity
    {
        public string GeoId { get; set; }

        public string DestinationId { get; set; }

        public string LandMarkCityDestinationId { get; set; }

        public string Type { get; set; }

        public string RedirectPage { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string SearchDetail { get; set; }

        public string Caption { get; set; }

        public string Name { get; set; }
    }
}
