using NServiceBus;

namespace LondonStockExnchange.DataProcessing.Write.Service
{
    public class TransactionPlacedHandler : IHandleMessages<object>
    {
        public Task Handle(object message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}