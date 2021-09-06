using Azure;
using Microsoft.Azure.Cosmos;
using ProjectB.Entities;
using ProjectB.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Core.Pipeline;

namespace ProjectB.Infrastructure
{
    public abstract class CosmosDbDataRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        protected readonly CosmosClient _client;

        public abstract string ContainerName { get; }

        public CosmosDbDataRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient client)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration;
            _client = client;
        }
        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                Container container = GetContainer();
                ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
                return createResponse.Resource;
            }
            catch (CosmosException)
            {
                throw;
            }
        }

        public async Task DeleteAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();

                await container.DeleteItemAsync<T>(entityId, new PartitionKey(entityId));
            }
            catch (CosmosException)
            {
                throw;
            }
        }

        public async Task<T> GetAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();

                ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));
                return entityResult.Resource;
            }
            catch (CosmosException)
            {
                throw;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                Container container = GetContainer();

                ItemResponse<BaseEntity> entityResult = await container
                                                           .ReadItemAsync<BaseEntity>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));

                if (entityResult != null)
                {
                    await container
                          .ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));
                }
                return entity;
            }
            catch (CosmosException)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return null;
            //try
            //{
            //    Container container = GetContainer();
            //    AsyncPageable<T> queryResultSetIterator = container.GetItemQueryIterator<T>;
            //    List<T> entities = new List<T>();

            //    await foreach (var entity in queryResultSetIterator)
            //    {
            //        entities.Add(entity);
            //    }

            //    return entities;

            //}
            //catch (CosmosException ex)
            //{
            //    return null;
            //}
        }


        protected Container GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            var container = database.GetContainer(ContainerName);
            return container;
        }
    }
}
