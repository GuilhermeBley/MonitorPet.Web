using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Json;

namespace MonitorPet.Ui.Client.Services
{
    public class AuthenticationStateService : AuthenticationStateProvider
    {
        public const string DEFAUL_LOCAL_STORAGE = "token_monitor_pet";
        private readonly JwtTokenService _jwtTokenService;
        private readonly LocalStorageService _localStorageService;
        private readonly HttpClient _client;

        public AuthenticationStateService(HttpClient client, JwtTokenService jwtTokenService, LocalStorageService localStorageService)
        {
            _jwtTokenService = jwtTokenService;
            _localStorageService = localStorageService;
            _client = client;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonymous = new ClaimsIdentity();

            var tokenStorage = await _localStorageService.GetItem(DEFAUL_LOCAL_STORAGE);

            if (string.IsNullOrWhiteSpace(tokenStorage))
                return new AuthenticationState(
                    new ClaimsPrincipal(anonymous));

            var tokenInfo = _jwtTokenService.GetTokenInfo(tokenStorage);

            if (tokenInfo.Expiration is null ||
                tokenInfo.IsExpired())
            {
                await _localStorageService.RemoveItem(DEFAUL_LOCAL_STORAGE);

                _client.DefaultRequestHeaders.Authorization = null;

                return new AuthenticationState(
                    new ClaimsPrincipal(anonymous));
            }

            const string DEFAULT_AUTH = "Bearer";

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue(DEFAULT_AUTH, tokenStorage);

            var identity = new ClaimsIdentity(DEFAULT_AUTH);

            identity.AddClaims(tokenInfo.Claims);

            return new AuthenticationState(
                new ClaimsPrincipal(identity));
        }
    }
}
