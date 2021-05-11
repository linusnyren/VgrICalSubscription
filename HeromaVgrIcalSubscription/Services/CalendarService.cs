using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using RestSharp;

namespace HeromaVgrIcalSubscription.Services
{
    public class CalendarService : ICalendarService
    {
        public CalendarService()
        {
        }

        public async Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months)
        {
            var client = new RestClient("https://heroma.vgregion.se/Webbklient/api/APCalendarApi/getCalendarFile");

            var request = new RestRequest(Method.POST);

            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long stop = DateTimeOffset.Now.AddMonths(months).ToUnixTimeMilliseconds();

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("Cookie", cookies.Token);
            request.AddParameter("StartString", now);
            request.AddParameter("StopString", stop);
            request.AddParameter("ShowWork", "true");
            request.AddParameter("ShowAbs", "true");
            request.AddParameter("ShowApp", "true");
            request.AddParameter("ShowTCall", "true");
            request.AddParameter("__RequestVerificationToken", cookies.VerificationToken);

            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }
    }
}
