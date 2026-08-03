using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace Order.API.Middlewares
{
    public class RedisRateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _redis;
        private readonly int _maxRequests = 20; // Límite de peticiones
        private readonly TimeSpan _timeWindow = TimeSpan.FromSeconds(60); // Ventana de tiempo

        public RedisRateLimitingMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
        {
            _next = next;
            _redis = redis;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var db = _redis.GetDatabase();
            
            // Identificar al cliente por su IP (o por su Token/UserId si está autenticado)
            var clientId = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var redisKey = $"ratelimit:{clientId}";

            // Incrementar contador de forma atómica en Redis
            var requestCount = await db.StringIncrementAsync(redisKey);

            if (requestCount == 1)
            {
                // Si es la primera petición en la ventana, asignamos la expiración
                await db.KeyExpireAsync(redisKey, _timeWindow);
            }

            if (requestCount > _maxRequests)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Rate limit exceeded. Too many requests.\" }");
                return;
            }

            await _next(context);
        }
    }
}