using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.class1.People
{
    public class Group_People
    {
        private readonly HttpClient _httpClient;

        public Group_People()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7182/api/People/Group_People/") };
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
        //*******READ********
        public async Task<List<DTO.People.Group_People>?> GetAll()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DTO.People.Group_People>>(json);
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
        public async Task<DTO.People.Group_People?> GetById(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DTO.People.Group_People>(json);
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
        public async Task<string> Add(DTO.People.Group_People aysnc)
        {
            try
            {
                AddAuthorizationHeader();
                var json = JsonConvert.SerializeObject(aysnc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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
        public async Task<string> Update(DTO.People.Group_People aysnc)
        {
            try
            {
                AddAuthorizationHeader();
                var json = JsonConvert.SerializeObject(aysnc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{aysnc.Id}", content);
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
