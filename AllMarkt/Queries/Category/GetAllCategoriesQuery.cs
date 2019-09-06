using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Queries.Category
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryViewModel>>
    {
    }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllCategoriesQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<CategoryViewModel>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _allMarktQueryContext
                .Categories
                .ToListAsync(cancellationToken);

            return from category in categories
                select new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
        }
    }

}
