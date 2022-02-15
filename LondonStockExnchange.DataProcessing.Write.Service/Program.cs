using LondonStockExnchange.DataProcessing.Write.Service.Infrastructure;
using LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NServiceBus;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<TransactionContext>(dbOptions => dbOptions.UseSqlServer("Server=localhost;Database=LondonStockExchange_Transactions_Writes;Integrated Security=SSPI;MultipleActiveResultSets=true"));
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    
    })
    .UseNServiceBus(ctx =>
    {
        var endpointConfiguration = new EndpointConfiguration("DataProcessing.Writer");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
