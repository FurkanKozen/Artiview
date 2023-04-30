using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Domain.Entities
{
    public class ReviewEntity
    {
        private const string ARGUMENT_EXCEPTION_MESSAGE = "Property cannot have default value";

        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }

        public ReviewEntity(Guid articleId, string reviewer, string reviewContent)
        {
            if (articleId == default)
                throw new ArgumentException(ARGUMENT_EXCEPTION_MESSAGE, nameof(articleId));
            if (string.IsNullOrWhiteSpace(reviewer))
                throw new ArgumentException(ARGUMENT_EXCEPTION_MESSAGE, nameof(reviewer));
            if (string.IsNullOrWhiteSpace(reviewContent))
                throw new ArgumentException(ARGUMENT_EXCEPTION_MESSAGE, nameof(reviewContent));

            ArticleId = articleId;
            Reviewer = reviewer;
            ReviewContent = reviewContent;
        }
        public ReviewEntity()
        {

        }

        public void Update(Guid? articleId, string reviewer, string reviewContent)
        {
            if (articleId.HasValue && articleId != default)
                ArticleId = articleId.Value;
            if (!string.IsNullOrWhiteSpace(reviewer))
                Reviewer = reviewer;
            if (!string.IsNullOrWhiteSpace(reviewContent))
                ReviewContent = reviewContent;
        }
    }
}
