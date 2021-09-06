using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {

            services.TryAddSingleton(implementationFactory =>
            {
                var cosmoDbConfiguration = implementationFactory.GetRequiredService<ICosmosDbConfiguration>();
                CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString);
                Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName)
                                                       .GetAwaiter()
                                                       .GetResult();
                database.CreateContainerIfNotExistsAsync(
                    cosmoDbConfiguration.HotelContainerName,
                    cosmoDbConfiguration.HotelContainerPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();
                
                return cosmosClient;
            });


            return services;
        }
    }
}
