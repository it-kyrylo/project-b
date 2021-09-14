using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using ProjectB.Clients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Repositories
{
    public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T: class
    {
        private Container _container;   
        public CosmosDbRepository(CosmosClient cosmosDbClient, IConfigurationSection configurationSection)
        {
            _container = cosmosDbClient.GetContainer(configurationSection["DatabaseName"], configurationSection["ContainerName"]);
        }
        public async Task AddAsync(T userInformation, string chatId)
        {
            await _container.CreateItemAsync(userInformation, new PartitionKey(chatId));
        }
        public async Task<T> GetAsync(string chatId)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(chatId, new PartitionKey(chatId));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateAsync(string chatId, T userInformation)
        {
            await _container.UpsertItemAsync(userInformation, new PartitionKey(chatId));
        }
        public async Task DeleteAsync(string chatId)
        {
            await _container.DeleteItemAsync<T>(chatId, new PartitionKey(chatId));
        }
    }
}
