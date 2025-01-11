using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Repositories;
using YahooFinanceScrapper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register db context
builder.Services.AddSqlServer<YahooFinanceDbContext>("Server=localhost;Database=yahoo_finance_db;Trusted_Connection=True;TrustServerCertificate=True;");

// Register repositories
builder.Services.AddTransient<ITickerRepository, TickerRepository>();
builder.Services.AddTransient<ITickerSymbolRepository, TickerSymbolRepository>();
builder.Services.AddTransient<IYahooFinanceScraperService, YahooFinanceScraperService>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
