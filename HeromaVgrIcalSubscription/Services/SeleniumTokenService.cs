using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace HeromaVgrIcalSubscription.Services
{
    public class SeleniumTokenService : ISeleniumTokenService
    {
        public SeleniumTokenService()
        {
        }

        public async Task<CookieModel> GetCookiesAsync(string username, string password)
        {
            IWebDriver driver;

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService("/Users/LinusNyren/Downloads", "geckodriver");
            service.Port = 64444;

            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments("--headless");

            driver = new FirefoxDriver(service, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://heroma.vgregion.se/Webbklient/Account/Login";

            driver.FindElement(By.XPath("//input[@placeholder='Användarnamn']")).SendKeys(username);
            driver.FindElement(By.XPath("//input[@placeholder='Lösenord']")).SendKeys(password);

            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            string verificationToken = driver.FindElement(By.XPath("//input[@name='__RequestVerificationToken']")).GetAttribute("value");
            var cookies = driver.Manage().Cookies.AllCookies;
            driver.Close();
            driver.Dispose();
            driver.Quit();
            string token = "";
            foreach(var cookie in cookies)
            {
                token += $"{cookie.Name}={cookie.Value}; ";
            }
            return new CookieModel
            {
                Token = token,
                VerificationToken = verificationToken
            };
        }
    }
}
