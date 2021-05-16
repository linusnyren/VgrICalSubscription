using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using HeromaVgrIcalSubscription.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HeromaVgrIcalSubscription.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchemaController : ControllerBase
    {

        private readonly ILogger<SchemaController> logger;
        private readonly ISchemaService schemaService;
        private readonly IMemoryCache cache;
        private readonly CacheOptions options;

        public SchemaController(
            ILogger<SchemaController> logger,
            ISchemaService schemaService,
            IMemoryCache cache,
            IOptions<CacheOptions> options
            )
        {
            this.logger = logger;
            this.schemaService = schemaService;
            this.cache = cache;
            this.options = options.Value;
        }
        [HttpGet]
        public async Task<string> GetHealth()
        {
            return "Hello there!";
        }

        [HttpGet("{user}/{password}/{months}")]
        public async Task<string> Get(string user, string password, int months)
        {
            logger.LogInformation("New incoming request from " + user);
            var req = new SchemaRequest
            {
                UserName = user,
                Password = password,
                Months = months
            };
            var key = $"{user}-{password}-{months}";
            return await cache.GetOrCreateAsync(key, async entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(options.TtlHours);
                return (await schemaService.GetCalendarAsync(req)).Content;
            });
        }
    }
}
