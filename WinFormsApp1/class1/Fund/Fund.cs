using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.class1.Fund
{
    public class Fund
    {
        private readonly HttpClient _httpClient;

        public Fund()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7182/api/Fund/Fund/") };
        }
        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(TokenStore.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenStore.Token);
            }
        }
        //******SEARCH*******
        //****** SEARCH *******
        public async Task<List<DTO.Fund.Fund>> SearchAsync(string name)
        {
            try
            {
                AddAuthorizationHeader();

                // اگر name خالی بود، رشته خالی بفرست
                string query = string.IsNullOrWhiteSpace(name)
                    ? "Search"
                    : $"Search?name={Uri.EscapeDataString(name)}";

                HttpResponseMessage response = await _httpClient.GetAsync(query);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"خطا در دریافت داده از سرور. ({response.StatusCode})");

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<DTO.Fund.Fund>>(json);

                return result ?? new List<DTO.Fund.Fund>();
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در جستجو: {ex.Message}");
            }
        }
        //*******READ********
        public async Task<List<DTO.Fund.Fund>?> GetAll()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DTO.Fund.Fund>>(json);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HttpRequestException: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<DTO.Fund.Fund?> GetById(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DTO.Fund.Fund>(json);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HttpRequestException: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
                return null;
            }
        }
        //*******CRUD********
        public async Task<string> Add(DTO.Fund.Fund Fund)
        {
            try
            {
                AddAuthorizationHeader();
                var json = JsonConvert.SerializeObject(Fund, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("", content);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return $"HttpRequestException: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
        public async Task<string> Update(DTO.Fund.Fund Fund)
        {
            try
            {
                AddAuthorizationHeader();
                var json = JsonConvert.SerializeObject(Fund, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{Fund.Id}", content);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return $"HttpRequestException: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
        public async Task<string> Delete(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"{id}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return $"HttpRequestException: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
