using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using RestSharp;

namespace HeromaVgrIcalSubscription.Services
{
    public class SchemaService : ISchemaService
    {
        private readonly ISeleniumTokenService seleniumTokenService;
        private readonly ICalendarService calendarService;

        public SchemaService(ISeleniumTokenService seleniumTokenService, ICalendarService calendarService)
        {
            this.seleniumTokenService = seleniumTokenService;
            this.calendarService = calendarService;
        }

        public async Task<IRestResponse> GetCalendarAsync(SchemaRequest req)
        {
            var cookies = await seleniumTokenService.GetCookiesAsync(req.UserName, req.Password);
            var res = await calendarService.GetIcalAsync(cookies, req.Months);

            return res;
        }
    }
}
