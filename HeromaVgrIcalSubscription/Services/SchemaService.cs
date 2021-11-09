using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using Ical.Net;
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
            var cookies = seleniumTokenService.GetCookiesAsync(req.UserName, req.Password);
            var res = await calendarService.GetIcalAsync(cookies, req.Months);
            Test(res.Content);
            return res;
        }

        private void Test(string content)
        {
            CalendarCollection icals = CalendarCollection.Load(content);
        }
    }
}
