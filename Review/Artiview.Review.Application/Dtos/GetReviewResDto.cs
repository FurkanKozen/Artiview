using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Application.Dtos
{
    public class GetReviewResDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }
    }
}
