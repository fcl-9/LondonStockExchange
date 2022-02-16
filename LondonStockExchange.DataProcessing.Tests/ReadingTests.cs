using FluentAssertions;
using LondonStockExchange.DataProcessing.Read.Api.Controllers;
using LondonStockExchange.DataProcessing.Read.Api.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LondonStockExchange.DataProcessing.Tests
{
    public class ReadingTests
    {
        private Mock<IStockRepository> mockStockRepository;
        private readonly StockController sut;

        public ReadingTests()
        {
            var mockLogger = new Mock<ILogger<StockController>>();
            mockStockRepository = new Mock<IStockRepository>();
            sut = new StockController(mockStockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GiveGetStockCurrentValue_WhenTickerSymbolIsInvalod_ThenBadRequestIsReturned()
        {
            var result = await sut.GetStockValueByTickerSymbolAsync(null);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GiveGetStockCurrentValue_WhenTickerSymbolIsValid_ThenOkIsReturned()
        {
            var tickerSymbol = "APPL";
            mockStockRepository.Setup(m => m.GetValueByTickerSymbol(tickerSymbol)).ReturnsAsync(new List<dynamic>() { "SOMEDATA" });

            var result = await sut.GetStockValueByTickerSymbolAsync(tickerSymbol);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        [InlineData(1, -1)]
        [InlineData(1, 51)]
        public async Task GivenGetAllStockCurrentValue_WhenPageSizeOrPageNumberAreInvalid_ThenBadRequestIsReturned(int pageNumber, int pageSize)
        {
            var result = await sut.GetAllStockValues(pageNumber, pageSize);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GivenGetAllStockCurrentValue_WhenPageSizeOrPageNumberAreValid_ThenOkIsReturned()
        {
            var pageSize = 1;
            var pageNumber = 2;
            mockStockRepository.Setup(m => m.GetValuesForAllTickers(pageNumber, pageSize)).ReturnsAsync(new List<dynamic>() { "SOMEDATA" });
         
            var result = await sut.GetAllStockValues(pageNumber, pageSize);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GivenGetStocksValuesByTickerSymbols_WhenTickerSymbolsAreInvalid_ThenBadRequestIsReturned()
        {
            var result = await sut.GetStockValueByTickerSymbols(new List<string>());
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GivenGetStocksValuesByTickerSymbols_WhenTickerSymbolsAreValid_ThenOkIsReturned()
        {
            var requestInput = new List<string>() { "APPL", "FB" };
            mockStockRepository.Setup(m => m.GetValuesByTickerSymbols(requestInput)).ReturnsAsync(new List<dynamic>() { "SOMEDATA" });

            var result = await sut.GetStockValueByTickerSymbols(requestInput);
            
            result.Result.Should().BeOfType<OkObjectResult>();
        }
    }
}
