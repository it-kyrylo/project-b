using ProjectB.Clients.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface ICosmosDbHotelInformationService
    {
        Task<IEnumerable<CosmosDbHotelInformation>> GetMultipleAsync(string query);
        Task<CosmosDbHotelInformation> GetAsync(string id);
        Task AddHotelIdAsync(CosmosDbHotelInformation userHotelInformation);
        Task AddCheckInDateAsync(CosmosDbHotelInformation userHotelInformation);
        Task AddCheckOutDateAsync(CosmosDbHotelInformation userHotelInformation);
        Task<string> GetHotelIdByChatIdAsync(string chatId);
        Task<string> GetCheckInDateByChatIdAsync(string chatId);
        Task<string> GetCheckOutDateByChatIdAsync(string chatId);
        Task UpdateAsync(string id, CosmosDbHotelInformation userHotelInformation);
        Task DeleteAsync(string id);
    }
}
