using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Article.Application.Dtos
{
    public class CreateArticleReqDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ArticleContent { get; set; }
    }
}
