using BasketBall.Client.Helpers;
using BasketBall.Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BasketBall.Client.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions defaultJsonSerializerOptions =>
           new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //Get method to get any type of data from webAPI
        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHTTP = await _httpClient.GetAsync(url);
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHTTP, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(response, true, responseHTTP);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, false, responseHTTP);
            }
        }
        //A Generic method used to create object: game/person/team
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {            
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, stringContent); // sending HttpPost to db
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response); //null on the response because i dont need to receive anything from server with Post<>
        }
        //a specific Post method to create Teams
        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }             
        }
        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, stringContent); // sending HttpPust to db
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }
        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}
