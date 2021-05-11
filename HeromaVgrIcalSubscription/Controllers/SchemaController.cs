using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
namespace HeromaVgrIcalSubscription.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchemaController : ControllerBase
    {

        private readonly ILogger<SchemaController> logger;
        private readonly ISchemaService schemaService;

        public SchemaController(ILogger<SchemaController> logger, ISchemaService schemaService)
        {
            this.logger = logger;
            this.schemaService = schemaService;
        }

        [HttpGet("{user}/{password}/{months}")]
        public async Task<string> Get(string user, string password, int months)
        {
            var req = new SchemaRequest
            {
                UserName = user,
                Password = password,
                Months = months
            };

            return (await schemaService.GetCalendarAsync(req)).Content;
            
        }
    }
}
