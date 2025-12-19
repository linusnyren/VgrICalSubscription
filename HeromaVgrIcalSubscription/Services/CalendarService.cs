
namespace HeromaVgrIcalSubscription.Services;

public interface ICalendarService
{
    Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months);
}

public class CalendarService : ICalendarService
{
    private readonly CalendarOptions _options;
    private readonly IRestClient _client;

    public CalendarService(IOptions<CalendarOptions> options, IRestClient client)
    {
        _options = options.Value;
        _client = client;
        client.BaseUrl = new(_options.URL);
    }

    public async Task<IRestResponse> GetIcalAsync(CookieModel cookies, int months)
    {
        var request = new RestRequest(Method.POST);

        var now = DateTimeOffset.Now.ToString("yyyy-MM-dd");

        var due = months < _options.MaxMonths ? months : _options.MaxMonths;
        var stop = DateTimeOffset.Now.AddMonths(due).ToString("yyyy-MM-dd");

        request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
        request.AddHeader("Cookie", cookies.Token);
        request.AddParameter("StartString", now);
        request.AddParameter("StopString", stop);
        request.AddParameter("ShowWork", "true");
        request.AddParameter("ShowAbs", "true");
        request.AddParameter("ShowApp", "true");
        request.AddParameter("ShowTCall", "true");
        request.AddParameter("ShowTaskWeekComments", "true");
        request.AddParameter("ShowTaskWeekStaffComments", "true");
        request.AddParameter("ShowWeekStaff", "true");
        request.AddParameter("__RequestVerificationToken", cookies.VerificationToken);


        return await _client.ExecuteAsync(request);
    }
}