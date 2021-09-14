using ProjectB.Clients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Repositories
{
    public interface ICosmosDbRepository <T>
    {
        Task AddAsync(T userInformation, string chatId);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string chatId);
        Task UpdateAsync(string chatId, T userInformation);
        Task DeleteAsync(string chatId);
    }
}
