using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service;
using StocksApp.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StocksApp.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TradingOptions _options;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _options = tradingOptions.Value;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),

                    Method = HttpMethod.Get,

                    //Headers = { new Dictionary <string, string> }
                };

                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponse.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);

                //the format of the response is json, so we have to convert it into a list
                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);



                if (responseDictionary != null)
                {
                    if (responseDictionary.ContainsKey("error"))
                    {
                        throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                    }
                    else
                    {
                        return responseDictionary;
                    }
                }
                else
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }

            };
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,

                    //Headers = { new Dictionary <string, string> }
                };

                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponse.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);

                //the format of the response is json, so we have to convert it into a list
                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);



                if (responseDictionary != null)
                {
                    if (responseDictionary.ContainsKey("error"))
                    {
                        throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                    }
                    else
                    {
                        return responseDictionary;
                    }
                }
                else
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }

            };
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}") //URI includes the secret token
            };

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();


            List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");

            return responseDictionary;
        }
    }
}
