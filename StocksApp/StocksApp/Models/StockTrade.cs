namespace StocksApp.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }
        public string? Image { get; set; }
        public string? Industry { get; set; }
        public string? Exchange { get; set; }
    }
}
