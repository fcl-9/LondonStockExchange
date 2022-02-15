using LondonStockExchange.DataProcessing.Contracts;
using LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository;
using NServiceBus;

namespace LondonStockExnchange.DataProcessing.Write.Service.Handler
{
    public class TransactionPlacedHandler : IHandleMessages<TransactionPlaced>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ILogger<TransactionPlacedHandler> logger;

        public TransactionPlacedHandler(ITransactionRepository transactionRepository, ILogger<TransactionPlacedHandler> logger)
        {
            this.transactionRepository=transactionRepository;
            this.logger=logger;
        }

        public Task Handle(TransactionPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation($"A new transaction has been placed at {message.PlacedDateTime} for ticker: {message.TickerSymbol}");
            transactionRepository.AddTransaction(message);
            return Task.CompletedTask;
        }
    }
}