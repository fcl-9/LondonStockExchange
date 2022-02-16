using LondonStockExchange.DataProcessing.Read.Api.Models;

namespace LondonStockExchange.DataProcessing.Read.Api.Infrastructure.Repository
{
    public interface IStockRepository
    {
        Task<StockTicker?> GetValueByTickerSymbol(string tickerSymbol);
        Task<IEnumerable<StockTicker>> GetValuesByTickerSymbols(IEnumerable<string> tickerSymbols);
        Task<IEnumerable<StockTicker>> GetValuesForAllTickers(int pageNumber, int pageSize);
    }
}
