using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Models;

namespace HeromaVgrIcalSubscription.Interfaces.Services
{
    public interface ISeleniumTokenService
    {
        Task<CookieModel> GetCookiesAsync(string username, string password);
    }
}
