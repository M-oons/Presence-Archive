using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Presence
{
    public static class API
    {
        private static readonly HttpClient _client;

        static API()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com")
            };
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); // Use GitHub API v3
            _client.DefaultRequestHeaders.Add("User-Agent", "M-oons/Presence"); // GitHub API requires User-Agent header
        }

        public static async Task<HttpResponseMessage> Get(string uri)
        {
            return await _client.GetAsync(uri);
        }
    }
}