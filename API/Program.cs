using API.BuilderFuctions;
using DB;
using DB.Repositories;
using DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Interfaces;
using WIMSFP.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IProductsRepo, ProductsRepo>();
builder.Services.AddScoped<IStockRepo, StockRepo>();
builder.Services.AddScoped<IDeliveryRepo, DeliveryRepo>();
builder.Services.AddScoped<IDishRepo, DishRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderObjRepo, OrderObjRepo>();
builder.Services.AddScoped<IDishIngredientRepo, DishIngredientRepo>();
builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IDishPriorityRepo, DishPriorityRepo>();

builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IDishPriorityService, DishPriorityService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenCreator, TokenCreator>();

builder.Services.AddHostedService<StockExpirationService>();
builder.Services.AddHostedService<DishPriorityRecalculationService>();

builder.SetupJWT();

builder.Services.AddAuthentication();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{ options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();  
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.Services.SeedAdminAsync();

app.UseCors("AllowAll");
app.MapControllers();
app.MapHub<DashboardHub>("/hubs/dashboard");
app.UseSwagger();
app.UseSwaggerUI();
app.Run();

