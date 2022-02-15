using LondonStockExchange.DataProcessing.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LondonStockExnchange.DataProcessing.Write.Service.Infrastructure
{
    public class TransactionContext: DbContext
    {
        public DbSet<TransactionPlaced> Transactions { get; set; }

        public TransactionContext(DbContextOptions<TransactionContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionPlaced>().HasKey(t => new { t.TickerSymbol, t.TradeDateTime });
        }
    }
}
