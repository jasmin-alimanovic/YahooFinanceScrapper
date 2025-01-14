using Microsoft.EntityFrameworkCore;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Repositories;

public class TickerRepository : ITickerRepository
{
    private readonly YahooFinanceDbContext dbContext;

    public TickerRepository(YahooFinanceDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Ticker>> GetAllAsync()
    {
        return await dbContext.Tickers.ToListAsync();
    }

    public async Task<Ticker?> GetAsync(PrimaryKey<Ticker> primaryKey)
    {
        return await dbContext.Tickers.FirstOrDefaultAsync(t => t.Id == primaryKey.Model.Id);
    }

    public async Task<Ticker?> GetByTickerAndDateAsync(string ticker, DateTime date)
    {
        return await dbContext.Tickers.FirstOrDefaultAsync(t => t.TickerName == ticker && t.Date.Equals(date));
    }
    public async Task<Ticker> CreateAsync(Ticker entity)
    {
        try
        {
            await dbContext.Tickers.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            var primaryKey = new PrimaryKey<Ticker>(entity);
            var createdObj = await GetAsync(primaryKey);

            if (createdObj is null)
            {
                throw new Exception("Failed to create Ticker");
            }

            return createdObj;
        }
        catch(Exception e)
        {
            throw;
        }

    }

    public Task DeleteAsync(PrimaryKey<Ticker> primaryKey)
    {
        throw new NotImplementedException();
    }

    public Task EditAsync(PrimaryKey<Ticker> primaryKey, Ticker entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Ticker?> GetBySymbolAndDate(string tickerSymbol, DateTime date)
    {
        return await dbContext.Tickers.FirstOrDefaultAsync(t => t.TickerName == tickerSymbol && t.Date.Value.Date.Equals(date.Date));
    }
}
