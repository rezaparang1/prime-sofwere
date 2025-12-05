using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.class1.Product
{
    public class Product
    {
        private readonly HttpClient _httpClient;

        public Product()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://85.9.123.232:5000/api/Product/Product/") };
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
        //public async Task<List<DTO.Product.Group_Product>?> GetAllAsync()
        //{
        //    try
        //    {
        //        AddAuthorizationHeader();
        //        var response = await _httpClient.GetAsync("");
        //        response.EnsureSuccessStatusCode();

        //        var json = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<List<DTO.Product.Group_Product>>(json);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        MessageBox.Show($"HttpRequestException: {ex.Message}");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception: {ex.Message}");
        //        return null;
        //    }
        //}
        //public async Task<DTO.Product.Group_Product?> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        AddAuthorizationHeader();
        //        var response = await _httpClient.GetAsync($"{id}");
        //        response.EnsureSuccessStatusCode();

        //        var json = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<DTO.Product.Group_Product>(json);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        MessageBox.Show($"HttpRequestException: {ex.Message}");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception: {ex.Message}");
        //        return null;
        //    }
        //}
        //*******CRUD********
        public async Task<string> AddAsync(ProductDtoForApi aysnc)
        {
            try
            {
                AddAuthorizationHeader();

                // 1) سریالایز کن و لاگ کن (مهم برای مقایسه با Postman)
                var json = JsonConvert.SerializeObject(aysnc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine(">>> Request JSON:");
                Debug.WriteLine(json);
                // یا برای نمایش به کاربر:
                // File.WriteAllText("last_request.json", json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 2) درخواست را بفرست ولی EnsureSuccessStatusCode را نزن
                var response = await _httpClient.PostAsync("", content);

                // 3) بخوان و لاگ کن وضعیت و بدنه
                var respBody = await response.Content.ReadAsStringAsync();
                var status = (int)response.StatusCode + " " + response.ReasonPhrase;
                Debug.WriteLine($">>> Response Status: {status}");
                Debug.WriteLine(">>> Response Body:");
                Debug.WriteLine(respBody);

                // 4) اگر 400 بود، برگرد پیام کامل برای دیدن ModelState
                if (!response.IsSuccessStatusCode)
                {
                    return $"Status: {status}\nBody: {respBody}";
                }

                return respBody;
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

        //public async Task<string> UpdateAsync(DTO.Product.Group_Product aysnc)
        //{
        //    try
        //    {
        //        AddAuthorizationHeader();
        //        var json = JsonConvert.SerializeObject(aysnc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PutAsync($"{aysnc.Id}", content);
        //        response.EnsureSuccessStatusCode();

        //        return await response.Content.ReadAsStringAsync();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return $"HttpRequestException: {ex.Message}";
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Exception: {ex.Message}";
        //    }
        //}
        //public async Task<string> DeleteAsync(int id)
        //{
        //    try
        //    {
        //        AddAuthorizationHeader();
        //        var response = await _httpClient.DeleteAsync($"{id}");
        //        response.EnsureSuccessStatusCode();

        //        return await response.Content.ReadAsStringAsync();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return $"HttpRequestException: {ex.Message}";
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Exception: {ex.Message}";
        //    }
        //}
    }

}
