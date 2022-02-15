using LondonStockExnchange.DataProcessing.Write.Service;
using NServiceBus;

IHost host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(ctx =>
    {
        var endpointConfiguration = new EndpointConfiguration("DataProcessing.Writer");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
