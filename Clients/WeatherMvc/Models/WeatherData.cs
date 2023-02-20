namespace WeatherMvc.Models
{
  public class WeatherData
  {
    public DateTime date { get; set; }
    public string? summary { get; set; }
    // public int temperatureF => 32 + (int)(TemperatureC / 0.5556);
    public int temperatureF { get; set; }
    public int temperatureC { get; set; }
  }
}