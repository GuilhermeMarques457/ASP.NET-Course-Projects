using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Core.Domain.RepositoryContracts;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Service.FinnhubService
{
    public class FinnhubServiceCompanyProfileGetter : IFinnhubServiceCompanyProfileGetter
    {
        
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubServiceCompanyProfileGetter(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);

            return responseDictionary;
        }

    }
}