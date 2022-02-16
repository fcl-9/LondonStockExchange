using LondonStockExchange.DataProcessing.Contracts;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.Title = Constants.IngestionService;
builder.Host.UseNServiceBus(ctx => {

    var endpointConfiguration = new EndpointConfiguration(Constants.IngestionService);
    var transport = endpointConfiguration.UseTransport<LearningTransport>();

    var conventions = endpointConfiguration.Conventions();
    conventions.DefiningCommandsAs(type => type == typeof(TransactionPlaced));

    transport.Routing().RouteToEndpoint(typeof(TransactionPlaced), Constants.WriterService);

    return endpointConfiguration;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
