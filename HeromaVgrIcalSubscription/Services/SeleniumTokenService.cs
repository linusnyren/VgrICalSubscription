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
            IWebDriver driver;

            /*FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(options.SeleniumDir, options.Driver);
            service.Port = options.ServicePort;

            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArguments("--headless");

            driver = new FirefoxDriver(service, firefoxOptions);*/
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>() {
                        "--silent-launch",
                        "--no-startup-window",
                        "no-sandbox",});

            var chromeDriverService = ChromeDriverService.CreateDefaultService(options.SeleniumDir, options.Driver);
            
            chromeDriverService.HideCommandPromptWindow = true;    // This is to hidden the console.

            driver = new ChromeDriver(chromeDriverService, chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(options.TimeOut);
            driver.Url = options.TargetUrl;

            driver.FindElement(By.XPath("//input[@placeholder='Username']")).SendKeys(username);
            driver.FindElement(By.XPath("//input[@placeholder='Password']")).SendKeys(password);
            
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
            return new CookieModel
            {
                Token = token,
                VerificationToken = verificationToken
            };
        }
    }
}
