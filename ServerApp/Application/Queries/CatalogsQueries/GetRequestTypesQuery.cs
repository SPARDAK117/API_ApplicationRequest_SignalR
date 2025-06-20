using Application.DTOs.ApplicationRequestDTOs;
using Application.DTOs.CatalogDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CatalogsQueries
{
    public record GetRequestTypesQuery : IRequest<List<CatalogRequestTypeDto>>;
}
