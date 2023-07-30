using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TradingOptions
    {
        public string? DefaultStockSymbol { get; set; }
        public string? StockSymbols { get; set; }
        public string? Top25PopularStocks { get; set; }
    }
}
