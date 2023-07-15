using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContracts;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Service
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TradingOptions _options;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
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
    }
}