using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherMvc.Models;
using System.Text.Json;

namespace WeatherMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Weather()
    {
      var data = new List<WeatherData>();

      using var client = new HttpClient();
      var result = await client.GetAsync("https://localhost:5003/WeatherForecast");

      if (result.IsSuccessStatusCode)
      {
        var model = await result.Content.ReadAsStringAsync();
        data = JsonSerializer.Deserialize<List<WeatherData>>(model);
        return View(data);
      }
      else
      {
        throw new Exception("Unable to get weather data.");
      }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
