using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artiview.Review.Application.Adapters
{
    public interface IApiAdapter
    {
        Task<bool> AnyArticleByIdAsync(Guid id);
    }
}
