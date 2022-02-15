using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonStockExnchange.DataProcessing.Write.Service.Infrastructure
{
    internal interface ITransactionRepository
    {
        public void AddTransaction(object transactionPlaced);
    }
}
