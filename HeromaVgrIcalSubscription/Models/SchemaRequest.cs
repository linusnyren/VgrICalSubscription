namespace HeromaVgrIcalSubscription.Models;

public class SchemaRequest(string userName, string password, int months)
{
    public string UserName { get; internal set; } = userName;
    public string Password { get; internal set; } = password;
    public int Months { get; internal set; } = months;
}