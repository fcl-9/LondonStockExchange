namespace LondonStockExchange.DataProcessing.Read.Api.Infrastructure.Repository
{
    public interface IStockRepository
    {
        Task<dynamic?> GetValueByTickerSymbol(string tickerSymbol);
        Task<IEnumerable<dynamic>> GetValuesByTickerSymbols(IEnumerable<string> tickerSymbols);
        Task<IEnumerable<dynamic>> GetValuesForAllTickers(int pageNumber, int pageSize);
    }
}
