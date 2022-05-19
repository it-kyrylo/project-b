namespace ProjectB.Repositories;

public interface ICosmosDbRepository <T>
{
    Task AddAsync(T userInformation, string chatId);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(string chatId);
    Task UpdateAsync(string chatId, T userInformation);
    Task DeleteAsync(string chatId);
}
