using ServiceContracts;
using StocksApp.Core.Domain.RepositoryContracts;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Service
{
    public class FinnhubServiceStockPriceGetter : IFinnhubServiceStockPriceGetter
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubServiceStockPriceGetter(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            return responseDictionary;
        }
    }
}