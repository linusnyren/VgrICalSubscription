
namespace HeromaVgrIcalSubscription.Services;

public interface ISeleniumTokenService
{
    CookieModel GetCookiesAsync(string username, string password);
}

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
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--whitelisted-ips=\"\"");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        using var driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub/"), chromeOptions);
        log.LogInformation("Connected to Selenium remote");

        driver.Navigate().GoToUrl(options.TargetUrl);
        log.LogInformation("Navigated to {OptionsTargetUrl}", options.TargetUrl);

        driver.FindElement(By.XPath("//input[@id='Username']")).SendKeys(username);
        driver.FindElement(By.XPath("//input[@id='Password']")).SendKeys(password);

        driver.FindElement(By.XPath("//button[@type='submit']")).Click();

        string verificationToken = driver.FindElement(By.XPath("//input[@name='__RequestVerificationToken']"))
            .GetAttribute("value");
        var cookies = driver.Manage().Cookies.AllCookies;

        driver.Close();
        driver.Quit();
        var token = cookies.Aggregate("", (current, cookie) => current + $"{cookie.Name}={cookie.Value}; ");

        if (!token.Contains("AspNetWebClientCookie_heroma.vgregion.se"))
            throw new ("Unsuccessful login");
        
        log.LogInformation("Succesfully retrieved tokens!");

        return new ()
        {
            Token = token,
            VerificationToken = verificationToken
        };
    }
}