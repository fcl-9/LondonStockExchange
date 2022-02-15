using LondonStockExchange.DataProcessing.Contracts;

namespace LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext context;

        public TransactionRepository(TransactionContext context)
        {
            this.context=context;
        }

        public void AddTransaction(TransactionPlaced transactionPlaced)
        {
            context.Add(transactionPlaced);
            context.SaveChanges();
        }
    }
}
