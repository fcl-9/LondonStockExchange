using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using LondonStockExchange.DataProcessing.Read.Api.Infrastructure.Repository;

namespace LondonStockExchange.DataProcessing.Read.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository stockRepository;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockRepository stockRepository, ILogger<StockController> logger)
        {
            this.stockRepository=stockRepository;
            _logger = logger;
        }

        [HttpGet("value")]
        public async Task<ActionResult<dynamic>> GetStockValueByTickerSymbolAsync(string tickerSymbol)
        {
            if(string.IsNullOrEmpty(tickerSymbol))
            {
                return BadRequest("TickerSymbol cannot be null or empty.");
            }

            var latestStockValue = await stockRepository.GetValueByTickerSymbol(tickerSymbol);
            if(latestStockValue == null)
            {
                return NotFound();
            }
            return Ok(latestStockValue);
        }

        [HttpGet("/stocks/value/all")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAllStockValues(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) { return BadRequest("PageNumber less than 1 is not allowed."); }
            if (pageSize <= 0){ return BadRequest("PageSize less than 1 is not allowed"); }
            if (pageSize > 50){ return BadRequest("PageSize more than 50 is not alloweded."); }

            var latestStockValues = await stockRepository.GetValuesForAllTickers(pageNumber, pageSize);
            if(!latestStockValues.Any())
            {
                return NotFound();
            }
            return Ok(latestStockValues);
        }

        [HttpGet("/stocks/value")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetStockValueByTickerSymbols([FromQuery] IEnumerable<string> tickerSymbols)
        {
            if(!tickerSymbols.Any())
            {
                return BadRequest("At least one TickerSymbol is required.");
            }

            var latestStockValues = await stockRepository.GetValuesByTickerSymbols(tickerSymbols);
            if(!latestStockValues.Any())
            {
                return NotFound();
            }
            return Ok(latestStockValues);
        }
    }
}