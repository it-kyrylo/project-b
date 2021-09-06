using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string HotelContainerName { get; set; }
        string HotelContainerPartitionKeyPath { get; set; }
    }
}
