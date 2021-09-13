using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectB.Services;
using Refit;
using System;
using ProjectB.Clients;
using Microsoft.Azure.Cosmos;
using Telegram.Bot;
using ProjectB.Handlers;
using ProjectB.Infrastructure;
using System.Threading.Tasks;

namespace ProjectB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddMemoryCache();
            services.AddSingleton(typeof(ICacheFilter<>), typeof(CacheFilter<>));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectB", Version = "v1" });
            });
            services.AddTransient<IHotelService, HotelService>();
            var apihost = Configuration["ApiConfiguration:ApiHost"];
            var apikey = Configuration["ApiConfiguration:ApiToken"];
            var apiurl = Configuration["ApiConfiguration:ApiUrl"];
            var telegramTokenApi = Configuration["TelegramBotConfiguration:Token"];

            services.AddRefitClient<IHotelClients>()
                .ConfigureHttpClient(c => c.DefaultRequestHeaders.Add("x-rapidapi-host",apihost))
                .ConfigureHttpClient(c => c.DefaultRequestHeaders.Add("x-rapidapi-key", apikey))
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiurl));
            services.AddAutoMapper(typeof(Startup));

            
            var database = InitializeCosmosDatabaseAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult();
            services.AddSingleton<ICosmosDbHotelInformationService>(InitializeHotelInformationContainerAsync(Configuration.GetSection("CosmosDb"), database).GetAwaiter().GetResult());
            services.AddSingleton<ICosmosDbUserHistoryService>(InitializeUserHistoryContainerAsync(Configuration.GetSection("CosmosDb"), database).GetAwaiter().GetResult());
           
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramTokenApi));
            services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();
            services.AddSingleton<IMessageBuilder, MessageBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static async Task<DatabaseResponse> InitializeCosmosDatabaseAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];
            var options = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            var client = new CosmosClient(account, key, options);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            return database;
        }
        private static async Task<CosmosDbHotelInformationService> InitializeHotelInformationContainerAsync(IConfigurationSection configurationSection, DatabaseResponse database)
        {
            var databaseName = configurationSection["DatabaseName"];
            var hotelInformationContainerName = configurationSection["HotelInformationContainerName"];
            await database.Database.CreateContainerIfNotExistsAsync(hotelInformationContainerName, "/id");
            var cosmosDbHotelInformationService = new CosmosDbHotelInformationService(database.Database.Client, databaseName, hotelInformationContainerName);
            return cosmosDbHotelInformationService;
        }
        private static async Task<CosmosDbUserHistoryService> InitializeUserHistoryContainerAsync(IConfigurationSection configurationSection, DatabaseResponse database)
        {
            var databaseName = configurationSection["DatabaseName"];
            var userHistoryContainerName = configurationSection["UserHistoryContainerName"];
            await database.Database.CreateContainerIfNotExistsAsync(userHistoryContainerName, "/id");
            var cosmosDbUserHistoryService = new CosmosDbUserHistoryService(database.Database.Client, databaseName, userHistoryContainerName);
            return cosmosDbUserHistoryService;
        }
    }
}
