namespace YahooFinanceScrapper.Models;

public class PrimaryKey<T>
{
    public T Model { get; set; }

    public PrimaryKey(T entity)
    {
        Model = entity;
    }
}
