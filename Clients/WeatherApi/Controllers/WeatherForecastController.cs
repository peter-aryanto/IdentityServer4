using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
/*
  curl -X POST -d "client_id=m2m&scope=weatherapi.read&client_secret=ClientCredentialsSecret&grant_type=client_credentials" "https://localhost:5001/connect/token"
  {"access_token":"eyJhbGciOiJSUzI1NiIsImtpZCI6IjkzRTdEQTA2MzlENTYyQjFBQ0I1QTYwODYzMjA2RDgzIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2NzY2MDg3MjAsImV4cCI6MTY3NjYxMjMyMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6IndlYXRoZXJhcGkiLCJjbGllbnRfaWQiOiJtMm0iLCJqdGkiOiIyRDEyQzNERDRGRkFBRjQzREVFMTY4Qzc1MUI0NjUxNiIsImlhdCI6MTY3NjYwODcyMCwic2NvcGUiOlsid2VhdGhlcmFwaS5yZWFkIl19.KhR3cQyuHbPeKDye_vguGj7H9NKargrQpk19c4EOFd852XBXAp_V-o53aZzbJnKgG1cI4X2Y_v0cmGQPV1BUWGz8BZ3y_tcZQ0en-8byoWpw4jTWPdCL8zOab-9yVCr9Tn2F8XR-vtZjAD2Lj9EHibAO0rRVQwYWZTt3weiT1oq9OHTeg_STHuB6QTrxRhWaSb3bbTItFuLoSnTSvZ0Q5wefa9B3-Hcg-aTQPJqY5wGOZ-uTPrWYx2eCqiqX4FEPNCTXy3Po4cuKbkfXLI0l96O3Czxxy6cLVz1YLCzDlFdCivBXCNz8eE6DwS59bNIvDtjk_b8XIpZMPf_ARAcVDw","expires_in":3600,"token_type":"Bearer","scope":"weatherapi.read"}

  curl -v -H "Authorization: Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjkzRTdEQTA2MzlENTYyQjFBQ0I1QTYwODYzMjA2RDgzIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2NzY2MDg3MjAsImV4cCI6MTY3NjYxMjMyMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6IndlYXRoZXJhcGkiLCJjbGllbnRfaWQiOiJtMm0iLCJqdGkiOiIyRDEyQzNERDRGRkFBRjQzREVFMTY4Qzc1MUI0NjUxNiIsImlhdCI6MTY3NjYwODcyMCwic2NvcGUiOlsid2VhdGhlcmFwaS5yZWFkIl19.KhR3cQyuHbPeKDye_vguGj7H9NKargrQpk19c4EOFd852XBXAp_V-o53aZzbJnKgG1cI4X2Y_v0cmGQPV1BUWGz8BZ3y_tcZQ0en-8byoWpw4jTWPdCL8zOab-9yVCr9Tn2F8XR-vtZjAD2Lj9EHibAO0rRVQwYWZTt3weiT1oq9OHTeg_STHuB6QTrxRhWaSb3bbTItFuLoSnTSvZ0Q5wefa9B3-Hcg-aTQPJqY5wGOZ-uTPrWYx2eCqiqX4FEPNCTXy3Po4cuKbkfXLI0l96O3Czxxy6cLVz1YLCzDlFdCivBXCNz8eE6DwS59bNIvDtjk_b8XIpZMPf_ARAcVDw" "https://localhost:5003/WeatherForecast"
  [{"date":"2023-02-18T15:53:26.0395216+11:00","temperatureC":3,"temperatureF":37,"summary":"Chilly"},{"date":"2023-02-19T15:53:26.0395321+11:00","temperatureC":-19,"temperatureF":-2,"summary":"Chilly"},{"date":"2023-02-20T15:53:26.0395325+11:00","temperatureC":-5,"temperatureF":24,"summary":"Scorching"},{"date":"2023-02-21T15:53:26.0395328+11:00","temperatureC":-15,"temperatureF":6,"summary":"Freezing"},{"date":"2023-02-22T15:53:26.0395331+11:00","temperatureC":31,"temperatureF":87,"summary":"Chilly"}]* Connection #0 to host localhost left intact
*/
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
