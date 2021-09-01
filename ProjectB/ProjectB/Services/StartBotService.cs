using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public class StartBotService : IHostedService
    {
        private const int OneMinuteInMilliseconds = 60000;
        //private readonly IServiceProvider _services;

        //public StartBotService(IServiceProvider services)
        //{
        //    _services = services;
        //}

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => SendNotifications(cancellationToken));
            return Task.CompletedTask;
        }

        private async void SendNotifications(CancellationToken stoppingToken)
        {
          //await SendMessages(_eventProvider.GetAllNotificationsToBeSentNow());
            await Task.Delay(OneMinuteInMilliseconds);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
