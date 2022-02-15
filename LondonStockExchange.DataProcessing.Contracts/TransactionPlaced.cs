namespace LondonStockExchange.DataProcessing.Contracts
{
    public class TransactionPlaced
    {
        public string TickerSymbol { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public decimal ShareNumber { get; set; }
        public string BrokerId { get; set; }
        public DateTime TradeDateTime { get; set; }
    }
}