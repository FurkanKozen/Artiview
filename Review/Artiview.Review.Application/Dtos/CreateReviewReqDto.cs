using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Application.Dtos
{
    public class CreateReviewReqDto
    {
        public Guid ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }
    }
}
