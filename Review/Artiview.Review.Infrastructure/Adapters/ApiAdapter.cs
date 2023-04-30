using Artiview.Review.Application.Adapters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Infrastructure.Adapters
{
    public class ApiAdapter : IApiAdapter
    {
        private readonly HttpClient _httpClient;
        public ApiAdapter(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<bool> AnyArticleByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Article/anyArticleById?id={id}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var validResult = bool.TryParse(responseContent, out bool result);

            if (!validResult)
                throw new HttpRequestException($"Unexpected response content type for: {responseContent}");

            return result;
        }
    }
}
