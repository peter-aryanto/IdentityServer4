using IdentityServer4.Test;
using System.Security.Claims;
using IdentityModel;
using System.Text.Json;
using IdentityServer4;
using IdentityServer4.Models;

namespace IDS
{
  public static class Config
  {
    public static List<TestUser> Users
    {
      get
      {
        // A dynamic object with custom properties.
        var address = new
        {
          street_address = "Dummy Street",
          locality = "Dummy Locality",
          postal_code = "Dummy Code",
          country = "Dummy Country",
        };

        return new List<TestUser>
        {
          new TestUser
          {
            SubjectId = "818001",
            Username = "User1",
            Password = "User1",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "User1"),
              new Claim(JwtClaimTypes.GivenName, "User"),
              new Claim(JwtClaimTypes.FamilyName, "One"),
              new Claim(JwtClaimTypes.Email, "User.One@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "admin"),
              new Claim(JwtClaimTypes.WebSite, "http://user1.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
            }
          },
          new TestUser
          {
            SubjectId = "818002",
            Username = "User2",
            Password = "User2",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "User2"),
              new Claim(JwtClaimTypes.GivenName, "User"),
              new Claim(JwtClaimTypes.FamilyName, "Two"),
              new Claim(JwtClaimTypes.Email, "User.Two@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "user"),
              new Claim(JwtClaimTypes.WebSite, "http://user2.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
            }
          },
        };
      }
    }

    private static readonly List<string> roleClaims = new List<string> {"role"};

    public static IEnumerable<IdentityResource> IdentityResources =>
      new[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource
        {
          Name = "role",
          UserClaims = roleClaims.ToList(),
        }
      };

    public static IEnumerable<ApiScope> ApiScopes =>
      new[]
      {
        new ApiScope("weatherapi.read"),
        new ApiScope("weatherapi.write"),
      };

    public static IEnumerable<ApiResource> ApiResources =>
      new[]
      {
        new ApiResource("weatherapi")
        {
          Scopes = ApiScopes.Select(scope => scope.Name).ToList(),
          ApiSecrets = new List<Secret> {new Secret("weatherapiSecret".Sha256())},
          UserClaims = roleClaims.ToList(),
        },
      };

    public static IEnumerable<Client> Clients =>
      new[]
      {
        // machine 2 machine client using credentials flow
        /*
          Example:
          curl -X POST -d "client_id=m2m&scope=weatherapi.read&client_secret=ClientCredentialsSecret&grant_type=client_credentials" "https://localhost:5001/connect/token"
          {
            "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjkzRTdEQTA2MzlENTYyQjFBQ0I1QTYwODYzMjA2RDgzIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2NzY2MDQzODgsImV4cCI6MTY3NjYwNzk4OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6IndlYXRoZXJhcGkiLCJjbGllbnRfaWQiOiJtMm0iLCJqdGkiOiJBOTExN0FDNTVFNTVEREJDNDEzQUQ2MDBFMEU1NEE0QyIsImlhdCI6MTY3NjYwNDM4OCwic2NvcGUiOlsid2VhdGhlcmFwaS5yZWFkIl19.qLgrjvq8QdL76KgXcQUqfJPgeEk14gO_gSJzvHikPqGvhrikCWexbrtxCqK47uWwkUblB5Z0-8bPzHzpjxjM0Pj8HcIpCLuxDY8wjequWB2sw8nx5asccAMIchBes-h0NP-X2szyrVuiLcFhx-Vv_N8Ofk0spZglV0OXyfRnRMQYo84Ex68CKpkvcf5NB2ZAqDMCfYb3lUYtBFbuasmCzF4HaQhylwmNm94JBbGgXDtPI5H9MpFkYoMNqXw0xQgGzqfCkt9LLuHfFmaSIAdH3b7bvfZzzyZwaCmKovrmaR9je3pPTzmFWCfjxN-ImIFJvLBuS79lzQ4zzdkSeyjU6Q",
            "expires_in": 3600,
            "token_type": "Bearer",
            "scope": "weatherapi.read"
          }
          Could be decoded in: jwt.ms or jwt.io
        */
        new Client
        {
          //asd//ClientId = "m2m.client",
          ClientId = "m2m",
          ClientName = "Client Credentials Client",
          AllowedGrantTypes = GrantTypes.ClientCredentials,
          ClientSecrets = {new Secret("ClientCredentialsSecret".Sha256())},
          AllowedScopes = ApiScopes.Select(scope => scope.Name).ToList(),
        },
        // interactive client using code flow + pkce
        new Client
        {
          ClientId = "interactive",

          AllowedGrantTypes = GrantTypes.Code,
          ClientSecrets = {new Secret("CodeSecret".Sha256())},
          AllowedScopes = {"openid", "profile", "weatherapi.read"},

          RedirectUris = {"https://localhost:5005/signin-oidc"},
          FrontChannelLogoutUri = "https://localhost:5005/signout-oidc",
          PostLogoutRedirectUris = {"https://localhost:5005/signout-callback-oidc"},

          AllowOfflineAccess = true,
          RequirePkce = true,
          RequireConsent = true,
          AllowPlainTextPkce = true,
        }
      };
  }
}