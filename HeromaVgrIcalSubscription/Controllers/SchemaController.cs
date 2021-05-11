using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
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
            var req = new Models.SchemaRequest
            {
                UserName = user,
                Password = password,
                Months = months
            };
            var res = await schemaService.GetCalendarAsync(req);

            return res.Content;
        }
    }
}
