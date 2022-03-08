using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using HeromaVgrIcalSubscription.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace HeromaVgrIcalSubscription.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly CalendarOptions options;
        private readonly IRestClient client;
        private readonly ILogger<CalendarService> log;

        public CalendarService(IOptions<CalendarOptions> options, IRestClient client, ILogger<CalendarService> log)
        {
            this.options = options.Value;
            this.client = client;
            this.log = log;
            client.BaseUrl = new Uri(this.options.URL);
        }

        public async Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months)
        {
            var request = new RestRequest(Method.POST);

            string now = DateTimeOffset.Now.ToString("yyyy-MM-dd");

            int due = months < options.MaxMonths ? months : options.MaxMonths;
            string stop = DateTimeOffset.Now.AddMonths(due).ToString("yyyy-MM-dd");

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("Cookie", cookies.Token);
            request.AddParameter("StartString", now);
            request.AddParameter("StopString", stop);
            request.AddParameter("ShowWork", "true");
            request.AddParameter("ShowAbs", "true");
            request.AddParameter("ShowApp", "true");
            request.AddParameter("ShowTCall", "true");
            request.AddParameter("ShowTaskWeekComments", "true");
            request.AddParameter("ShowTaskWeekStaffComments", "true");
            request.AddParameter("ShowWeekStaff", "true");
            request.AddParameter("__RequestVerificationToken", cookies.VerificationToken);
            

            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }
    }
}
