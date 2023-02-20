using IdentityModel.Client;

namespace WeatherMvc.Services
{
  public interface ITokenService
  {
    // Task<TokenResponse> GetToken(string scope);
    Task<string> GetToken(string scope);
  }
}