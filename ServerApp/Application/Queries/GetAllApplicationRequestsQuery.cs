using Application.DTOs.ApplicationRequestDTOs;
using MediatR;

namespace Application.Queries
{
    public record GetAllApplicationRequestsQuery() : IRequest<List<ApplicationRequestDto>>;
}
