using ProjectB.Infrastructure;
using ProjectB.Clients.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface ICosmosDbUserHistoryService
    {
        Task<IEnumerable<CosmosDbUserHistory>> GetMultipleAsync(string query);
        Task<CosmosDbUserHistory> GetAsync(string id);
        Task AddToHistoryAsync(CosmosDbUserHistory userSearchHistory);
        Task<IEnumerable<string>> GetHistoryAsync(string chatId);
        Task UpdateAsync(string id, CosmosDbUserHistory userSearchHistory);
        Task DeleteAsync(string id);
    }
}
