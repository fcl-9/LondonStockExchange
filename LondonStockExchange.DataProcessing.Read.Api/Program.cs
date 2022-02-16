using LondonStockExchange.DataProcessing.Contracts;
using LondonStockExchange.DataProcessing.Read.Api.Infrastructure.Repository;

Console.Title = Constants.ReaderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStockRepository>(r => new StockRepository("Server=localhost;Database=LondonStockExchange_Transactions_Writes;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True"));

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
