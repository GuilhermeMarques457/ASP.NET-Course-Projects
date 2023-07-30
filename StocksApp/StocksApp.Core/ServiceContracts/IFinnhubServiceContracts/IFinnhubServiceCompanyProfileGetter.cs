namespace ServiceContracts
{
    public interface IFinnhubServiceCompanyProfileGetter
    {

        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

    }
}