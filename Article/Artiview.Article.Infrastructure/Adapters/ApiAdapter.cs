using Artiview.Article.Application.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Artiview.Article.Infrastructure.Adapters
{
    public class ApiAdapter : IApiAdapter
    {
        private readonly HttpClient _httpClient;
        public ApiAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AnyReviewByArticleIdAsync(Guid articleId)
        {
            var response = await _httpClient.GetAsync($"api/Review/anyReviewByArticleId?articleId={articleId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var validResult = bool.TryParse(responseContent, out bool result);

            if (!validResult)
                throw new HttpRequestException($"Unexpected response content type for: {responseContent}");

            return result;
        }
    }
}
