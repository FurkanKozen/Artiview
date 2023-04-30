using Artiview.Review.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Application.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<ReviewEntity>> GetReviewsAsync();
        Task<ReviewEntity> GetReviewByIdAsync(Guid id);
        Task<bool> AnyReviewByArticleIdAsync(Guid articleId);
        Task CreateReviewAsync(ReviewEntity reviewEntity);
        Task UpdateReviewAsync(ReviewEntity reviewEntity);
        Task DeleteReviewAsync(Guid id);
    }
}
