using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReversiMvcApp.Lib;

namespace ReversiMvcApp.Services
{
    public class Service<T> : IService<T> where T : new()
    {
        private readonly ApiClient _apiClient;
        public Service(IConfiguration configuration, IHttpClientFactory factory)
        {
            // Onderstaande komt uit: https://www.aspsnippets.com/Articles/Using-IConfiguration-in-ASPNet-Core.aspx
            // What is IConfiguration
            // The IConfiguration is an interface for .Net Core 2.0.
            // The IConfiguration interface need to be injected as dependency in the Controller and then later used throughout the Controller.
            // The IConfiguration interface is used to read Settings and Connection Strings from AppSettings.json file.
            var _configuration = configuration;
            var _connectionstring = _configuration.GetValue<string>("ApiUri");

            var _factory = factory;

            _apiClient = new ApiClient(_factory, _connectionstring);
        }

        public async Task<bool> AddAsync(T item, string path)
        {

            var response = await _apiClient.Post(path, item);
            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task<bool> DeleteAsync(int id, string path)
        {
            var response = await _apiClient.Delete(path, id.ToString());
            if (response.IsSuccessStatusCode) return true;
            return false;
        }


        public async Task<List<T>> GetAsync(string path)
        {
            var items = new List<T>();
            var response = await _apiClient.Get(path, "");
            if (response.IsSuccessStatusCode) items = await response.Content.ReadAsAsync<List<T>>(); // Microsoft.AspNet.WebApi.Client needed
            return items;
        }

        public async Task<T> GetAsync(int id, string path)
        {
            T item = new();
            var response = await _apiClient.Get(path, id.ToString());
            if (response.IsSuccessStatusCode) item = await response.Content.ReadAsAsync<T>();
            return item;
        }

        public async Task<bool> UpdateAsync(int id, T item, string path)
        {
            var response = await _apiClient.Put(path, id.ToString(), item);
            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task<bool> UpdateSpecialAsync(int id, object item, string path)
        {
            var response = await _apiClient.Put(path, id.ToString(), item);
            if (response.IsSuccessStatusCode) return true;
            return false;
        }
    }
}

