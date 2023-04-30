using Artiview.Article.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Article.Application.Adapters
{
    public interface IApiAdapter
    {
        Task<bool> AnyReviewByArticleIdAsync(Guid articleId);
    }
}
