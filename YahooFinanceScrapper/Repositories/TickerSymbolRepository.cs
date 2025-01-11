using Microsoft.EntityFrameworkCore;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Repositories;

public class TickerSymbolRepository(YahooFinanceDbContext dbContext) : ITickerSymbolRepository
{
    public async Task<List<TickerSymbol>> GetAllAsync()
    {
        return await dbContext.TickerSymbols.ToListAsync();
    }

    public async Task<TickerSymbol?> GetAsync(PrimaryKey<TickerSymbol> primaryKey)
    {
        return await dbContext.TickerSymbols.FirstOrDefaultAsync(t => t.Symbol == primaryKey.Model.Symbol);
    }
    public async Task<TickerSymbol> CreateAsync(TickerSymbol entity)
    {
        await dbContext.TickerSymbols.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        var primaryKey = new PrimaryKey<TickerSymbol>(entity);
        var createdObj = await GetAsync(primaryKey);

        if (createdObj is null)
        {
            throw new Exception("Failed to create TickerSymbol");
        }

        return createdObj;
    }

    public Task DeleteAsync(PrimaryKey<TickerSymbol> primaryKey)
    {
        throw new NotImplementedException();
    }

    public Task EditAsync(PrimaryKey<TickerSymbol> primaryKey, TickerSymbol entity)
    {
        throw new NotImplementedException();
    }

}
