using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region Index

        [Fact]
        public async Task Trade_ToReturnView()
        {

            HttpResponseMessage response = await _httpClient.GetAsync("/Trade/Index/MSFT");

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            var document = html.DocumentNode;

            var price = document.QuerySelector(".price");

            response.Should().BeSuccessful(); //2xx

            price.Should().NotBeNull();

        }
        #endregion
    }
}
