using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using HeromaVgrIcalSubscription.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

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

        public CookieModel GetCookiesAsync(string username, string password)
        {
            //IWebDriver driver;
            //var capability = new DesiredCapabilities();
            //driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub/"), capability, TimeSpan.FromSeconds(600));

            FirefoxOptions firefoxOptions = new FirefoxOptions();
            IWebDriver driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub/"), firefoxOptions);
            driver.Navigate().GoToUrl(options.TargetUrl);

            driver.FindElement(By.XPath("//input[@id='Username']")).SendKeys(username);
            driver.FindElement(By.XPath("//input[@id='Password']")).SendKeys(password);
            
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

            if (!token.Contains("AspNetWebClientCookie_heroma.vgregion.se"))
                throw new Exception("Unsuccessful login");
            else
                log.LogInformation("Succesfully retrieved tokens!");

            return new CookieModel
            {
                Token = token,
                VerificationToken = verificationToken
            };
        }
    }
}
