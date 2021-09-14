using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface ICosmosDbService<T>
    { 
        Task AddAsync(T userInformation);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string chatId);
        Task AddToHistoryAsync(T userInformation, string searchToAdd);
        Task AddHotelIdAsync(T userInformation);
        Task AddCheckInDateAsync(T userInformation);
        Task AddCheckOutDateAsync(T userInformation);
        Task<IEnumerable<string>> GetHistoryAsync(string chatId);
        Task<string> GetHotelIdByChatIdAsync(string chatId);
        Task<string> GetCheckInDateByChatIdAsync(string chatId);
        Task<string> GetCheckOutDateByChatIdAsync(string chatId);
        Task UpdateAsync(string chatId, T userInformation);
        Task DeleteAsync(string chatId);
    }
}
