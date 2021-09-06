using ProjectB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface IDataRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T newEntity);
        Task<T> GetAsync(string entityId);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(string entityId);
        Task<IReadOnlyList<T>> GetAllAsync();
    }
}
