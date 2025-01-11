using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetAsync(PrimaryKey<T> primaryKey);
    Task<T> CreateAsync(T entity);
    Task EditAsync(PrimaryKey<T> primaryKey, T entity);
    Task DeleteAsync(PrimaryKey<T> primaryKey);
}
