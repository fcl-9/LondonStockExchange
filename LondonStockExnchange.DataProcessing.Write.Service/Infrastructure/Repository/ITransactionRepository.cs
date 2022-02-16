using LondonStockExchange.DataProcessing.Contracts;

namespace LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository
{
    public interface ITransactionRepository
    {
        public void AddTransaction(TransactionPlaced transactionPlaced);
    }
}
