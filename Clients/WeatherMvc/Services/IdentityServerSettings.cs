namespace WeatherMvc.Services
{
  public class IdentityServerSettings
  {
    public string? DiscoveryUrl { get; set; }
    public string? ClientName { get; set; }
    public string? ClientPassword { get; set; }
    public bool UseHttps { get; set; }
  }

/*
  public class InteractiveSettings
  {
    public string? AuthorityUrl { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Scopes;
  }
*/
}