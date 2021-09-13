using Microsoft.Azure.Cosmos;
using ProjectB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public class CosmosDbUserHistoryService : ICosmosDbUserHistoryService
    {
        private Container _container;
        public CosmosDbUserHistoryService(CosmosClient cosmosDbClient, string databaseName, string userHistoryContainerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, userHistoryContainerName);
        }
        public async Task AddToHistoryAsync(CosmosDbUserHistory userSearchHistory)
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
        public async Task<IEnumerable<string>> GetHistoryAsync(string chatId)
        {
            if (await CheckIfExists(chatId) == true)
            {
                var chat = await GetAsync(chatId);
                List<string> searchHistory = chat.SearchHistory.Split(", ").ToList();
                return searchHistory;
            }
            return null;
        }
        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<CosmosDbUserHistory>(id, new PartitionKey(id));
        }
        public async Task<CosmosDbUserHistory> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CosmosDbUserHistory>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<CosmosDbUserHistory>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<CosmosDbUserHistory>(new QueryDefinition(queryString));
            var results = new List<CosmosDbUserHistory>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateAsync(string id, CosmosDbUserHistory userSearchHistory)
        {
            await _container.UpsertItemAsync(userSearchHistory, new PartitionKey(id));
        }
        public async Task<bool> CheckIfExists(string chatId)
        {
            var userToCheck = await GetAsync(chatId);
            if (userToCheck == null)
            {
                throw new Exception("User not found!");
            }
            return true;
        }
    }
}
