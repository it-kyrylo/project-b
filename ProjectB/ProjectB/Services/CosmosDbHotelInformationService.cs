using Microsoft.Azure.Cosmos;
using ProjectB.Clients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public class CosmosDbHotelInformationService : ICosmosDbHotelInformationService
    {
        private Container _container;
        public CosmosDbHotelInformationService(CosmosClient cosmosDbClient, string databaseName, string hotelInformationContainerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, hotelInformationContainerName);
        }
        public async Task AddHotelIdAsync(CosmosDbHotelInformation userHotelInformation)
        {
            var userId = userHotelInformation.Id;
            var hotelInformationToEdit = await GetAsync(userId);

            if (hotelInformationToEdit != null)
            {      
                string hotelIdToAdd = userHotelInformation.HotelId;
                string newHotelId = hotelIdToAdd;
                hotelInformationToEdit.HotelId = newHotelId;
                await UpdateAsync(userId, hotelInformationToEdit);
                return;
            }
            await _container.CreateItemAsync(userHotelInformation, new PartitionKey(userHotelInformation.Id));
        }
        public async Task AddCheckInDateAsync(CosmosDbHotelInformation userHotelInformation)
        {
            var userId = userHotelInformation.Id;
            var hotelInformationToEdit = await GetAsync(userId);

            if (hotelInformationToEdit != null)
            {
                string CheckInDateToAdd = userHotelInformation.CheckInDate;
                string newCheckInDate = CheckInDateToAdd;
                hotelInformationToEdit.CheckInDate = newCheckInDate;
                await UpdateAsync(userId, hotelInformationToEdit);
                return;
            }
            await _container.CreateItemAsync(userHotelInformation, new PartitionKey(userHotelInformation.Id));
        }
        public async Task AddCheckOutDateAsync(CosmosDbHotelInformation userHotelInformation)
        {
            var userId = userHotelInformation.Id;
            var hotelInformationToEdit = await GetAsync(userId);

            if (hotelInformationToEdit != null)
            {
                string CheckOutDateToAdd = userHotelInformation.CheckOutDate;
                string newCheckOutDate = CheckOutDateToAdd;
                hotelInformationToEdit.CheckOutDate = newCheckOutDate;
                await UpdateAsync(userId, hotelInformationToEdit);
                return;
            }
            await _container.CreateItemAsync(userHotelInformation, new PartitionKey(userHotelInformation.Id));
        }
        public async Task<string> GetHotelIdByChatIdAsync(string chatId)
        {
            if (await CheckIfExists(chatId) == true)
            {
                var chatInfo = await GetAsync(chatId);
                var hotelId = chatInfo.HotelId;
                return hotelId;
            }
            return null;
        }
        public async Task<string> GetCheckInDateByChatIdAsync(string chatId)
        {
            if (await CheckIfExists(chatId) == true)
            {
                var chatInfo = await GetAsync(chatId);
                var checkInDate = chatInfo.CheckInDate;
                return checkInDate;
            }
            return null;
        }
        public async Task<string> GetCheckOutDateByChatIdAsync(string chatId)
        {
            if (await CheckIfExists(chatId) == true)
            {
                var chatInfo = await GetAsync(chatId);
                var checkOutDate = chatInfo.CheckOutDate;
                return checkOutDate;
            }
            return null;
        }
        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<CosmosDbHotelInformation>(id, new PartitionKey(id));
        }
        public async Task<CosmosDbHotelInformation> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CosmosDbHotelInformation>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<CosmosDbHotelInformation>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<CosmosDbHotelInformation>(new QueryDefinition(queryString));
            var results = new List<CosmosDbHotelInformation>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateAsync(string id, CosmosDbHotelInformation userHotelInformation)
        {
            await _container.UpsertItemAsync(userHotelInformation, new PartitionKey(id));
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
