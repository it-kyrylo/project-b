using ProjectB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<CosmosDbSearchHistory>> GetMultipleAsync(string query);
        Task<CosmosDbSearchHistory> GetAsync(string id);
        Task AddAsync(CosmosDbSearchHistory hotelInformation);
        Task UpdateAsync(string id, CosmosDbSearchHistory hotelInformation);
        Task DeleteAsync(string id);
    }
}
