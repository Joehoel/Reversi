using System;
using System.Net.Http;

namespace ReversiMvcApp.Services
{

    public class ApiService
    {
        private readonly HttpClient httpClient;

        public ApiService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001/");
        }

    }

}

