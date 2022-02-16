using LondonStockExchange.DataProcessing.Contracts;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace LondonStockExchange.Ingestion.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngestController : ControllerBase
    {
        private readonly IMessageSession messageSession;
        private readonly ILogger<IngestController> logger;

        public IngestController(IMessageSession messageSession, ILogger<IngestController> logger)
        {
            this.messageSession=messageSession;
            this.logger = logger;
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> IngestTradeEvent()
        {
            var randomNumber = new Random();
            for (int i = 0; i < 10; i++)
            {

                var transactionPlacedEvent = new TransactionPlaced()
                {
                    TickerSymbol = i % 2 == 0 ? "APPL": "AMZN",
                    Price = randomNumber.Next(),
                    Currency = "GBP",
                    ShareNumber = randomNumber.Next(),
                    BrokerId = i / 5 > 0 ? "BROK1" : "BROK2",
                    TradeDateTime = DateTime.UtcNow
                };
                await messageSession.Send(transactionPlacedEvent);
                logger.LogInformation($"EVENT: TransactionPlaced, Ticker:{transactionPlacedEvent.TickerSymbol} Time:{transactionPlacedEvent.TradeDateTime}");
            }
            return Ok();
        }
    }
}