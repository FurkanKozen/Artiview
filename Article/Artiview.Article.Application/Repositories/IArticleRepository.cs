using Artiview.Article.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Article.Application.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<ArticleEntity>> GetArticlesAsync();
        Task<ArticleEntity> GetArticleByIdAsync(Guid id);
        Task<bool> AnyArticleByIdAsync(Guid id);
        Task CreateArticleAsync(ArticleEntity articleEntity);
        Task UpdateArticleAsync(ArticleEntity articleEntity);
        Task DeleteArticleAsync(Guid id);
    }
}
