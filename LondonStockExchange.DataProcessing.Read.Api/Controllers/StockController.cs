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
        public async Task<ActionResult<dynamic>> GetStockValueByTickerSymbolAsync(string tickerSymbol)
        {
            if(string.IsNullOrEmpty(tickerSymbol))
            {
                return BadRequest("TickerSymbol cannot be null or empty.");
            }

            var sql = @"SELECT TOP 1 [TickerSymbol], [TradeDateTime], [Price],[Currency] 
                        FROM[LondonStockExchange_Transactions_Writes].[dbo].[Transactions]
                        WHERE TickerSymbol = @TickerSymbol
                        ORDER BY TradeDateTime DESC";
            
            await using var connection = new SqlConnection("Server=localhost;Database=LondonStockExchange_Transactions_Writes;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True");
            await connection.OpenAsync();

            var lastStockPrice = await connection.QueryFirstOrDefaultAsync<object>(sql, new { TickerSymbol = tickerSymbol});

            return Ok(lastStockPrice);
        }


        [HttpGet("/stocks/value/all")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAllStockValues(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) { return BadRequest("PageNumber less than 1 is not allowed."); }
            if (pageSize <= 0){ return BadRequest("PageSize less than 1 is not allowed"); }
            if (pageSize > 50){ return BadRequest("PageSize more than 50 is not alloweded."); }
            
            var sql = @"Select t1.TickerSymbol, t1.Price, t1.Currency, t1.TradeDateTime
                        from [LondonStockExchange_Transactions_Writes].[dbo].[Transactions] as t1
                        inner join 
                        (
                            SELECT t2.TickerSymbol, MAX(t2.[TradeDateTime]) as LastestDate
		                    FROM [LondonStockExchange_Transactions_Writes].[dbo].[Transactions] as t2
		                    Group by t2.TickerSymbol 
					        Order By t2.TickerSymbol
					        OFFSET (@PageNumber-1)*@PageSize ROWS
					        FETCH NEXT @PageSize ROWS ONLY
                        ) t 
                        on t1.TradeDateTime = t.LastestDate
                        and t1.TickerSymbol = t.TickerSymbol";

            await using var connection = new SqlConnection("Server=localhost;Database=LondonStockExchange_Transactions_Writes;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True");
            await connection.OpenAsync();

            var lastStockPricesForTickers = await connection.QueryAsync<object>(sql, new { PageNumber = pageNumber, PageSize = pageSize });

            return Ok(lastStockPricesForTickers);
        }

        [HttpGet("/stocks/value")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetStockValueByTickerSymbols([FromQuery] IList<string> tickerSymbols)
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
           
            return Ok(lastStockPricesForTickers);
        }
    }
}