using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LondonStockExchange.DataProcessing.Read.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
       private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
        }

        [HttpGet("value")]
        public async Task<dynamic> GetStockValueByTickerSymbolAsync(string tickerSymbol)
        {
            var sql = @"SELECT TOP 1 [TickerSymbol], [TradeDateTime], [Price],[Currency] 
                        FROM[LondonStockExchange_Transactions_Writes].[dbo].[Transactions]
                        WHERE TickerSymbol = @TickerSymbol
                        ORDER BY TradeDateTime DESC";
            
            await using var connection = new SqlConnection("Server=localhost;Database=LondonStockExchange_Transactions_Writes;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True");
            await connection.OpenAsync();

            var lastStockPrice = await connection.QueryAsync<object>(sql, new { TickerSymbol = tickerSymbol});

            return Ok(lastStockPrice);
        }

        //[HttpGet("value/all")]
        //public IEnumerable<dynamic> GetAllStockValues()
        //{
        //    //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    //{
        //    //    Date = DateTime.Now.AddDays(index),
        //    //    TemperatureC = Random.Shared.Next(-20, 55),
        //    //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    //})
        //    //.ToArray();
        //    throw new NotImplementedException();
        //}

        [HttpGet("/stocks/values")]
        public async Task<IEnumerable<dynamic>> GetStockValueByTickerSymbols([FromQuery] IList<string> tickerSymbols)
        {
            var sql = @"
                Select t1.TickerSymbol, t1.Price, t1.Currency, t1.TradeDateTime
                from [LondonStockExchange_Transactions_Writes].[dbo].[Transactions] as t1
                inner join 
                (
                    SELECT t2.TickerSymbol, MAX(t2.[TradeDateTime]) as LastestDate
		            FROM [LondonStockExchange_Transactions_Writes].[dbo].[Transactions] as t2
		            WHERE t2.TickerSymbol In @TickerSymbols
		            Group by t2.TickerSymbol 
                ) t 
                on t1.TradeDateTime = t.LastestDate
                and t1.TickerSymbol = t.TickerSymbol
                ";

            await using var connection = new SqlConnection("Server=localhost;Database=LondonStockExchange_Transactions_Writes;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True");
            await connection.OpenAsync();

            var lastStockPricesForTickers = await connection.QueryAsync<object>(sql, new { TickerSymbols = tickerSymbols.ToArray()});
            throw new NotImplementedException();
        }
    }
}