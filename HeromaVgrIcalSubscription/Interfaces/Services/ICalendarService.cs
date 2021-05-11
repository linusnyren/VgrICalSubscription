using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Models;
using RestSharp;

namespace HeromaVgrIcalSubscription.Interfaces.Services
{
    public interface ICalendarService
    {
        Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months);
    }
}
