using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public record GetAllApplicationRequestsQuery() : IRequest<List<ApplicationRequestDto>>;
}
