namespace ServiceContracts
{
    public interface IFinnhubServiceStockPriceGetter
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}