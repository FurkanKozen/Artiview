using Artiview.Review.Application.Repositories;
using Artiview.Review.Domain.Entities;
using Artiview.Review.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewDbContext _reviewDbContext;

        public ReviewRepository(ReviewDbContext reviewDbContext)
        {
            _reviewDbContext = reviewDbContext;
        }

        public async Task CreateReviewAsync(ReviewEntity reviewEntity)
        {
            await _reviewDbContext.Reviews.AddAsync(reviewEntity);
            await _reviewDbContext.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(Guid id)
        {
            var review = await GetReviewByIdAsync(id);
            _reviewDbContext.Reviews.Remove(review);
            await _reviewDbContext.SaveChangesAsync();
        }

        public async Task<bool> AnyReviewByArticleIdAsync(Guid articleId)
        {
            return await _reviewDbContext.Reviews.AnyAsync(r => r.ArticleId == articleId);
        }

        public async Task<IEnumerable<ReviewEntity>> GetReviewsAsync()
        {
            return await _reviewDbContext.Reviews.ToListAsync();
        }
        public async Task<ReviewEntity> GetReviewByIdAsync(Guid id)
        {
            if (id == default)
                throw new ArgumentException($"{nameof(id)} cannot be {id}");

            return await _reviewDbContext.Reviews.FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new ArgumentException($"{nameof(ReviewEntity)} with {nameof(id)}={id} cannot be found");
        }

        public async Task UpdateReviewAsync(ReviewEntity reviewEntity)
        {
            _reviewDbContext.Reviews.Update(reviewEntity);
            await _reviewDbContext.SaveChangesAsync();
        }
    }
}
