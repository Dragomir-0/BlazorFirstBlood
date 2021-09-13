using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using BlazorFirstBlood.Client.Services;

namespace BlazorFirstBlood.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient http;
        private readonly IBananaService bananaService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http,
            IBananaService bananaService)
        {
            this.localStorageService = localStorageService;
            this.http = http;
            this.bananaService = bananaService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await this.localStorageService.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();
            this.http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    this.http.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"",""));
                    await this.bananaService.GetBananas();
                }
                catch (Exception)
                {
                    await this.localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }


        //Chris' Implementation
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            return claims;
        }
    }
}
