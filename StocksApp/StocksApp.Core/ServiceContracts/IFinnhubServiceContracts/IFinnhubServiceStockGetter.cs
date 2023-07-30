namespace ServiceContracts
{
    public interface IFinnhubServiceStockGetter
    {
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}