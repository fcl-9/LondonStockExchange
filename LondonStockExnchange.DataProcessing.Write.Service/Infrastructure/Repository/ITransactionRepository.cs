using LondonStockExchange.DataProcessing.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository
{
    public interface ITransactionRepository
    {
        public void AddTransaction(TransactionPlaced transactionPlaced);
    }
}
