using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Article.Domain.Entities
{
    public class ArticleEntity
    {
        private const string ARGUMENT_EXCEPTION_MESSAGE = "Property cannot have default value";

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ArticleContent { get; set; }
        public DateTime PublishDate { get; set; }
        public int StarCount { get; set; }

        public ArticleEntity(string title, string author, string articleContent)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(ARGUMENT_EXCEPTION_MESSAGE, nameof(title));
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException(ARGUMENT_EXCEPTION_MESSAGE, nameof(author));
            if (string.IsNullOrWhiteSpace(articleContent))
                throw new ArgumentException(ARGUMENT_EXCEPTION_MESSAGE, nameof(articleContent));

            PublishDate = DateTime.Now;
            Title = title;
            Author = author;
            ArticleContent = articleContent;
        }
        public ArticleEntity()
        {

        }

        public void Update(string title, string author, string articleContent, DateTime? publishDate, int? starCount)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            if (!string.IsNullOrWhiteSpace(author))
                Author = author;

            if (!string.IsNullOrWhiteSpace(articleContent))
                ArticleContent = articleContent;

            if (publishDate.HasValue)
                PublishDate = publishDate.Value;

            if (starCount.HasValue)
                StarCount = starCount.Value;
        }
    }
}
