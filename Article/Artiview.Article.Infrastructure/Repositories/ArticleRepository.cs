using Artiview.Article.Application.Repositories;
using Artiview.Article.Domain.Entities;
using Artiview.Article.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Article.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleDbContext _articleDbContext;
        public ArticleRepository(ArticleDbContext articleDbContext)
        {
            _articleDbContext = articleDbContext;
        }

        public async Task<IEnumerable<ArticleEntity>> GetArticlesAsync()
        {
            return await _articleDbContext.Articles.ToListAsync();
        }
        public async Task<ArticleEntity> GetArticleByIdAsync(Guid id)
        {
            if (id == default)
                throw new ArgumentException($"{nameof(id)} cannot be {id}");

            return await _articleDbContext.Articles.FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new ArgumentException($"{nameof(ArticleEntity)} with {nameof(id)}={id} cannot be found");
        }

        public async Task CreateArticleAsync(ArticleEntity articleEntity)
        {
            await _articleDbContext.Articles.AddAsync(articleEntity);
            await _articleDbContext.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            var article = await GetArticleByIdAsync(id);
            _articleDbContext.Articles.Remove(article);
            await _articleDbContext.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(ArticleEntity articleEntity)
        {
            _articleDbContext.Articles.Update(articleEntity);
            await _articleDbContext.SaveChangesAsync();
        }

        public async Task<bool> AnyArticleByIdAsync(Guid id)
        {
            return await _articleDbContext.Articles.AnyAsync(a => a.Id == id);
        }
    }
}
