using FluentAssertions;
using HtmlAgilityPack;
using Tests;
using Xunit;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;

namespace SampleApplicationCRUD
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }


        #region Index

        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrage

            //Act

            HttpResponseMessage response = await _httpClient.GetAsync("/Persons/Index");

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            var document = html.DocumentNode;

            var table = document.QuerySelector("table.people-table");

           

            //Assert
            response.Should().BeSuccessful(); //2xx

            table.Should().NotBeNull();



         }
        #endregion
    }
}