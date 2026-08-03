using Order.Application.Common.Interfaces;
using Order.Domain.Repositories;
using Order.Infrastructure.Messaging;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Repositories;
using Order.Infrastructure.Caching;
using StackExchange.Redis;
using Order.API.Middlewares;
using Order.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddControllers();

builder.Services.AddScoped<OrderDbContext>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IEventProducer, KafkaEventProducer>();
builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
                options.InstanceName = "OrderSystem_";
            });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RedisRateLimitingMiddleware>();
app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();