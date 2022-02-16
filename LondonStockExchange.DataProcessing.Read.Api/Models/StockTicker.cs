namespace LondonStockExchange.DataProcessing.Read.Api.Models
{
    public class StockTicker
    {
        public string TickerSymbol { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateTime TradeDateTime { get; set; }
    }
}
