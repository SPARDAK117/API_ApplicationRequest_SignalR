using Application.DTOs.CatalogDTOs;
using Application.Queries.CatalogsQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.CatalogHandler
{
    public class GetAllRequestTypeCatalogHandler(AppDbContext context) : IRequestHandler<GetRequestTypesQuery, List<CatalogRequestTypeDto>>
    {
        public async Task<List<CatalogRequestTypeDto>> Handle(GetRequestTypesQuery request, CancellationToken cancellationToken)
        {
            var result = await context.RequestTypes
                .Select(rt => new CatalogRequestTypeDto
                {
                    Id = rt.Id,
                    Name = rt.Name
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}

