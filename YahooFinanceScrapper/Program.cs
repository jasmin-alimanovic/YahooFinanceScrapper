using Microsoft.EntityFrameworkCore;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Repositories;
using YahooFinanceScrapper.Services;

var builder = WebApplication.CreateBuilder(args);

// Register db context
builder.Services.AddDbContext<YahooFinanceDbContext>(options =>
{
    options.UseSqlServer("Server=localhost;Database=yahoo_finance_db;Trusted_Connection=True;TrustServerCertificate=True;");
}, ServiceLifetime.Singleton);

// Register repositories
builder.Services.AddScoped<ITickerRepository, TickerRepository>();
builder.Services.AddScoped<ITickerSymbolRepository, TickerSymbolRepository>();

// Register services
builder.Services.AddScoped<IYahooFinanceScraperService, YahooFinanceScraperService>();
builder.Services.AddScoped<ITickerSymbolService, TickerSymbolService>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tickers}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
