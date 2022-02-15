namespace LondonStockExnchange.DataProcessing.Write.Service.Infrastructure
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext context;

        public TransactionRepository(TransactionContext context)
        {
            this.context=context;
        }

        public void AddTransaction(object transactionPlaced)
        {
            context.Add(transactionPlaced);
            context.SaveChanges();
        }
    }
}
