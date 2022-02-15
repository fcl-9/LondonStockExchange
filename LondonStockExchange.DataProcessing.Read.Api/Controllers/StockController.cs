using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<dynamic> GetStockValueByTickerSymbol(string tickerSymbol)
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            throw new NotImplementedException();
        }

        [HttpGet("value/all")]
        public IEnumerable<dynamic> GetAllStockValues()
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            throw new NotImplementedException();
        }

        [HttpGet("value")]
        public IEnumerable<dynamic> GetStockValueByTickerSymbols(IList<string> tickerSymbols)
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            throw new NotImplementedException();
        }
    }
}