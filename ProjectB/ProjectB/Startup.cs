using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProjectB.Services;
using Refit;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectB.Clients;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using System.Threading;
using SimpleInjector;
using ProjectB.Handlers;

namespace ProjectB
{
    public class Startup
    {
        private readonly Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            services.AddSimpleInjector(this._container, options =>
            {
                options.AddHostedService<StartBotService>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectB", Version = "v1" });
            });
            services.AddTransient<IHotelService, HotelService>();
            var apihost = Configuration["ApiConfiguration:ApiHost"];
            var apikey = Configuration["ApiConfiguration:ApiToken"];
            var apiurl = Configuration["ApiConfiguration:ApiUrl"];
            services.AddRefitClient<IHotelClients>()
                .ConfigureHttpClient(c => c.DefaultRequestHeaders.Add("x-rapidapi-host",apihost))
                .ConfigureHttpClient(c => c.DefaultRequestHeaders.Add("x-rapidapi-key", apikey))
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiurl));
            services.AddAutoMapper(typeof(Startup));
            InitializeContainer();
        }

        private void InitializeContainer()
        {
            _container.Register(typeof(TelegramUpdateHandler));
            _container.Register<ITelegramUpdateHandler, TelegramUpdateHandler>();
            _container.RegisterInstance(CreateClient(Configuration["TelegramBotConfiguration:Token"]));
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

            this._container.Verify();

            InitializeTelegramListener();
        }

        private ITelegramBotClient CreateClient(string apikey) => new TelegramBotClient(apikey);

        private async void InitializeTelegramListener()
        {
            var handler = this._container.GetInstance<ITelegramUpdateHandler>();
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var source = new CancellationTokenSource();
            await this._container.GetInstance<ITelegramBotClient>().ReceiveAsync(new DefaultUpdateHandler(handler.HandleUpdateAsync, handler.HandleErrorAsync), source.Token);
        }
    }
}
