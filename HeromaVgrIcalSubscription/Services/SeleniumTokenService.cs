using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using HeromaVgrIcalSubscription.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace HeromaVgrIcalSubscription.Services
{
    public class SeleniumTokenService : ISeleniumTokenService
    {
        private readonly SeleniumOptions options;
        private readonly ILogger<SeleniumTokenService> log;

        public SeleniumTokenService(IOptions<SeleniumOptions> options, ILogger<SeleniumTokenService> log)
        {
            this.options = options.Value;
            this.log = log;
        }

        public async Task<CookieModel> GetCookiesAsync(string username, string password)
        {
            IWebDriver driver;

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(options.SeleniumDir, options.Driver);
            service.Port = options.ServicePort;

            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArguments("--headless");

            driver = new FirefoxDriver(service, firefoxOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(options.TimeOut);
            driver.Url = options.TargetUrl;
            if (options.Locale == "SV")
            {
                driver.FindElement(By.XPath("//input[@placeholder='Användarnamn']")).SendKeys(username);
                driver.FindElement(By.XPath("//input[@placeholder='Lösenord']")).SendKeys(password);
            }
            else if (options.Locale == "EN")
            {
                driver.FindElement(By.XPath("//input[@placeholder='Username']")).SendKeys(username);
                driver.FindElement(By.XPath("//input[@placeholder='Password']")).SendKeys(password);
            }

            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            string verificationToken = driver.FindElement(By.XPath("//input[@name='__RequestVerificationToken']")).GetAttribute("value");
            var cookies = driver.Manage().Cookies.AllCookies;
            log.LogInformation(verificationToken);
            driver.Close();
            driver.Dispose();
            driver.Quit();
            string token = "";
            foreach(var cookie in cookies)
            {
                if (cookie.Name == "language")
                    token += "language=sv-SE; ";
                else
                    token += $"{cookie.Name}={cookie.Value}; ";
            }
            log.LogInformation(token);
            return new CookieModel
            {
                Token = token,
                VerificationToken = verificationToken
            };
        }
    }
}
