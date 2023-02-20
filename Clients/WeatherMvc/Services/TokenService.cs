using Microsoft.Extensions.Options;
using IdentityModel.Client;
// using Microsoft.Identity.Client;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherMvc.Services
{
  public class TokenService : ITokenService
  {
    private readonly DiscoveryDocumentResponse _discoDocResp;

    // Cache using manual initialisation.
    private readonly MemoryCache _tokenCache = new MemoryCache(new MemoryCacheOptions());
    // Cache using DI (will have an instance assigned in the constructor).
    // private readonly IMemoryCache _tokenCache;

    private readonly IOptions<IdentityServerSettings> _identityServerSettings;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
      IOptions<IdentityServerSettings> identityServerSettings,
      // IMemoryCache memoryCache,
      ILogger<TokenService> logger
    )
    {
      _identityServerSettings = identityServerSettings;
      // _tokenCache = memoryCache;
      _logger = logger;

      using var client = new HttpClient();
      _discoDocResp = client.GetDiscoveryDocumentAsync(_identityServerSettings.Value.DiscoveryUrl).Result;
      if (_discoDocResp.IsError)
      {
        _logger.LogError($"Unable to get discovery document. Error: {_discoDocResp.Error}");
        throw new Exception("Unable to get discovery document.", _discoDocResp.Exception);
      }
    }

    // public async Task<TokenResponse> GetToken(string scope)
    public async Task<string> GetToken(string scope)
    {
      if (_tokenCache.TryGetValue(scope, out string token))
      {
        return token;
      }
///*
      using var client = new HttpClient();
      var tokenResponse = await client.RequestClientCredentialsTokenAsync(
        new ClientCredentialsTokenRequest
        {
          Address = _discoDocResp.TokenEndpoint,
          ClientId = _identityServerSettings.Value.ClientName,
          ClientSecret = _identityServerSettings.Value.ClientPassword,
          Scope = scope,
        }
      );

      if (tokenResponse.IsError)
      {
        _logger.LogError($"Unable to get token. Error: {tokenResponse.Error}");
        throw new Exception("Unable to get token.", tokenResponse.Exception);
      }

      //return tokenResponse;
      var newToken = tokenResponse.AccessToken;
      var tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 30);
      _tokenCache.Set(scope, newToken, tokenExpiry);
      return newToken;
//*/
/*
      var clientApp = ConfidentialClientApplicationBuilder.Create(_identityServerSettings.Value.ClientName)
        .WithClientSecret(_identityServerSettings.Value.ClientPassword)
        .WithAuthority("https://localhost:5001/connect/token")
        .Build();
      var authResult = await clientApp.AcquireTokenForClient(new List<string> { scope })
        .ExecuteAsync();

      return null;
*/
    }

    // public async Task<string> GetToken(string scope)
    // {
    // }
  }
}