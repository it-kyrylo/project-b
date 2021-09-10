using Microsoft.Azure.Cosmos;
using ProjectB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        public CosmosDbService(CosmosClient cosmosDbClient, string databaseName, string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddAsync(CosmosDbSearchHistory userSearchHistory)
        {
            var searchId = userSearchHistory.Id;
            var oldSearchesToEdit = await GetAsync(searchId);

            if (oldSearchesToEdit != null)
            {
                string oldSearches = oldSearchesToEdit.SearchHistory;
                string searchToAdd = userSearchHistory.SearchHistory;
                string newSearches = oldSearches + $", {searchToAdd}";
                oldSearchesToEdit.SearchHistory = newSearches;
                await UpdateAsync(searchId, oldSearchesToEdit);
                return;
            }
            await _container.CreateItemAsync(userSearchHistory, new PartitionKey(userSearchHistory.Id));
        }
        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<CosmosDbSearchHistory>(id, new PartitionKey(id));
        }
        public async Task<CosmosDbSearchHistory> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CosmosDbSearchHistory>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<CosmosDbSearchHistory>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<CosmosDbSearchHistory>(new QueryDefinition(queryString));
            var results = new List<CosmosDbSearchHistory>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateAsync(string id, CosmosDbSearchHistory userSearchHistory)
        {
            await _container.UpsertItemAsync(userSearchHistory, new PartitionKey(id));
        }
    }
}
