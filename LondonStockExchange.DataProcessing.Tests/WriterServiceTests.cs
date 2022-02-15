using LondonStockExchange.DataProcessing.Contracts;
using LondonStockExnchange.DataProcessing.Write.Service.Handler;
using LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using NServiceBus.Testing;

namespace LondonStockExchange.DataProcessing.Tests
{
    public class WriterServiceTests
    {
        [Fact]
        public void GivenMessagePublishToQueue_ThenWriterService_StoresMessageInQueue()
        {
            var logger = new Mock<ILogger<TransactionPlacedHandler>>();
            var transactionRepository = new Mock<ITransactionRepository>();
            var sut = new TransactionPlacedHandler(transactionRepository.Object, logger.Object);

            var transactionPlacedEvent = new TransactionPlaced()
            {
                TickerSymbol = "APPL",
                Price = 10,
                Currency = "GBP",
                ShareNumber = 1,
                BrokerId = "BROK1",
                TradeDateTime = new DateTime(2020,5,5)
            };

            sut.Handle(transactionPlacedEvent, new TestableMessageHandlerContext());

            transactionRepository.Verify(s => s.AddTransaction(transactionPlacedEvent), Times.Once);
        }
    }
}