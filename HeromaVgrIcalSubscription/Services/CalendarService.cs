using RestSharp;

namespace HeromaVgrIcalSubscription.Services;

public interface ICalendarService
{
    Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months);
}

public class CalendarService(IOptions<CalendarOptions> options, IRestClient client) : ICalendarService
{
    private readonly IRestClient _client = client;
    private readonly CalendarOptions _options = options.Value;

    public async Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months)
    {
        _client.BaseUrl ??= new(_options.URL);

        var now = DateTimeOffset.Now.ToString("yyyy-MM-dd");

        var due = Math.Min(months, _options.MaxMonths);
        var stop = DateTimeOffset.Now.AddMonths(due).ToString("yyyy-MM-dd");

        var request = GetBaseRequest()
            .AddHeader("Cookie", cookies.Token)
            .AddParameter("StartString", now)
            .AddParameter("StopString", stop)
            .AddParameter("__RequestVerificationToken", cookies.VerificationToken);

        return await _client.ExecuteAsync(request);
    }

    private static IRestRequest GetBaseRequest()
    {
        return new RestRequest(Method.POST)
            .AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
            .AddParameter("ShowWork", "true")
            .AddParameter("ShowAbs", "true")
            .AddParameter("ShowApp", "true")
            .AddParameter("ShowTCall", "true")
            .AddParameter("ShowTaskWeekComments", "true")
            .AddParameter("ShowTaskWeekStaffComments", "true")
            .AddParameter("ShowWeekStaff", "true");
    }
}