using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Service;
using ServiceContracts;
using StocksApp.Controllers;
using StocksApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    
    public class StockControllerTest
    {
        private readonly IFinnhubServiceStockGetter _finnhubServiceStockGetter;
        private readonly IFinnhubServiceCompanyProfileGetter _finnhubServiceCompanyProfileGetter;
        private readonly IFinnhubServiceStockPriceGetter _finnhubServiceStockPriceGetter;
        private readonly Mock<IFinnhubServiceStockGetter> _finnhubServiceStockGetterMock;
        private readonly Mock<IFinnhubServiceCompanyProfileGetter> _finnhubServiceCompanyProfileGetterMock;
        private readonly Mock<IFinnhubServiceStockPriceGetter> _finnhubServiceStockPriceGetterMock;

        private readonly ILogger<TradeController> _logger;
        private readonly Mock<ILogger<TradeController>> _loggerMock;
        private readonly Fixture _fixture;
        
        

        public StockControllerTest()
        {
            _loggerMock = new Mock<ILogger<TradeController>>();
            _logger = _loggerMock.Object;
            _fixture = new Fixture();

            _finnhubServiceCompanyProfileGetterMock = new Mock<IFinnhubServiceCompanyProfileGetter>();
            _finnhubServiceStockGetterMock = new Mock<IFinnhubServiceStockGetter>();
            _finnhubServiceStockPriceGetterMock = new Mock<IFinnhubServiceStockPriceGetter>();

            _finnhubServiceStockGetter = _finnhubServiceStockGetterMock.Object;
            _finnhubServiceStockPriceGetter = _finnhubServiceStockPriceGetterMock.Object;
            _finnhubServiceCompanyProfileGetter = _finnhubServiceCompanyProfileGetterMock.Object;


        }

        #region Explore
        [Fact]
        public async Task Explore_NullSearch_ShouldReturnAllStocks()
        {
            IOptions<TradingOptions> tradingOptions = Options.Create(new TradingOptions() {
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO" }
            );

            StocksController stocksController = new StocksController(_finnhubServiceStockGetter,_finnhubServiceCompanyProfileGetter, _finnhubServiceStockPriceGetter, tradingOptions, _logger);

            List<Dictionary<string, string>>? stocksDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");

            _finnhubServiceStockGetterMock.Setup(temp => temp.GetStocks())
                .ReturnsAsync(stocksDict);

            var expectedStocks = stocksDict!
                .Select(temp => new StockVM() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();

            IActionResult result = await stocksController.Explore(null, null);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<StockVM>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(expectedStocks);


        }


        #endregion
    }
}
