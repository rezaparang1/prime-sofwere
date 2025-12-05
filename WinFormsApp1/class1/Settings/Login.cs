using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace WinFormsApp1.class1.Settings
{
    public  class Login
    {
        private  readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7182/")
        };

        public  async Task<TokenResponse?> LoginAsync(string userName, string password)
        {
            var dto = new { UserName = userName, Password = password };
            var res = await _http.PostAsJsonAsync("api/Auth/login", dto);

            if (!res.IsSuccessStatusCode)
                return null;

            var token = await res.Content.ReadFromJsonAsync<TokenResponse>();
            if (token == null) return null;

            TokenStore.Token = token.Token;
            TokenStore.ExpiresAt = token.ExpiresAt;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);

            return token;
        }

        //public static async Task<HttpResponseMessage> GetAsync(string url)
        //{
        //    EnsureAuthHeader();
        //    return await _http.GetAsync(url);
        //}

        //public static async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T body)
        //{
        //    EnsureAuthHeader();
        //    return await _http.PostAsJsonAsync(url, body);
        //}

        private  void EnsureAuthHeader()
        {
            if (!string.IsNullOrWhiteSpace(TokenStore.Token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenStore.Token);
            }
        }
    }
}
