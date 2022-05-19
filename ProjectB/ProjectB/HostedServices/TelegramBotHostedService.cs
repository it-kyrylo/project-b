using Telegram.Bot.Extensions.Polling;

namespace ProjectB.HostedServices
{
    public class TelegramBotHostedService : IHostedService, IDisposable
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ITelegramUpdateHandler _telegramUpdateHandler;

        public TelegramBotHostedService(ITelegramBotClient telegramBotClient, ITelegramUpdateHandler telegramUpdateHandler)
        {
            _telegramBotClient = telegramBotClient;
            _telegramUpdateHandler = telegramUpdateHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _telegramBotClient.ReceiveAsync(new DefaultUpdateHandler(_telegramUpdateHandler.HandleUpdateAsync,
                _telegramUpdateHandler.HandleErrorAsync), cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _telegramBotClient.CloseAsync(cancellationToken);
        }

        public void Dispose()
        {
            // todo: implement it
        }
    }
}