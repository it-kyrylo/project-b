using Microsoft.AspNetCore.Mvc;
using ProjectB.TelegramBot;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace ProjectB.Controllers
{
    [ApiController]
    [Route("api/bot")] //https:port/api/bot
    public class TelegramBotController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update) // FromBody means get update from body section of HTTP request
        {                                                   // Update update is what we receive from Telegram

            TelegramBotClient client = new TelegramBotClient("TokenGoesHere");
            HandleUpdateService handleUpdateService = new HandleUpdateService(client);
            await handleUpdateService.EchoAsync(update);


            /* When someone sends message to Bot, Telegram sends update object to url set in the webhook.
            //Just send message with text "answer" to the same chat(Update.Message.From.Id)
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                await client.SendTextMessageAsync(update.Message.From.Id,"answer");
            } */

            return Ok();
        }
    }
}
