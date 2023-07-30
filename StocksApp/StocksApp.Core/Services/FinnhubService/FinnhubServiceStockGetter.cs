using ServiceContracts;
using StocksApp.Core.Domain.RepositoryContracts;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Service
{
    public class FinnhubServiceStockGetter : IFinnhubServiceStockGetter
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubServiceStockGetter(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }


       
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            List<Dictionary<string, string>>? responseDictionary = await _finnhubRepository.GetStocks();

            return responseDictionary;
        }

    }
}