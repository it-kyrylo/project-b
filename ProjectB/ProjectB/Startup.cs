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
using Telegram.Bot;
using ProjectB.Handlers;
using ProjectB.Infrastructure;


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
            services.AddScoped(typeof(ICacheFilter<>), typeof(CacheFilter<>));
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

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramTokenApi));
            services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();
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
    }
}
