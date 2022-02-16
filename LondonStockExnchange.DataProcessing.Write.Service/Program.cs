using LondonStockExchange.DataProcessing.Contracts;
using LondonStockExnchange.DataProcessing.Write.Service.Infrastructure;
using LondonStockExnchange.DataProcessing.Write.Service.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NServiceBus;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {        
        services.AddDbContext<TransactionContext>(dbOptions => dbOptions.UseSqlServer(hostContext.Configuration.GetConnectionString("Database")));
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    
    })
    .UseNServiceBus(ctx =>
    {
        Console.Title = Constants.WriterService;
        var endpointConfiguration = new EndpointConfiguration(Constants.WriterService);
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
