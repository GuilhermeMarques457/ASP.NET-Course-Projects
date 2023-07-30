using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _context;

        public StockRepository(StockDbContext context)
        {
            _context = context;
        }

        public async Task<BuyOrder> BuyOrder(BuyOrder buyOrder)
        {
            await _context.BuyOrder.AddAsync(buyOrder);
            await _context.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _context.BuyOrder.Select(o => o).ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _context.SellOrder.Select(o => o).ToListAsync();
        }

        public async Task<SellOrder> SellOrder(SellOrder sellOrder)
        {
            await _context.SellOrder.AddAsync(sellOrder);
            await _context.SaveChangesAsync();
            return sellOrder;
        }
    }
}