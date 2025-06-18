using Application.DTOs;
using Application.Queries;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Application.Handlers
{
    public class GetAllApplicationRequestsHandler : IRequestHandler<GetAllApplicationRequestsQuery, List<ApplicationRequestDto>>
    {
        private readonly IApplicationRequestRepository _repository;

        public GetAllApplicationRequestsHandler(IApplicationRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApplicationRequestDto>> Handle(GetAllApplicationRequestsQuery request, CancellationToken cancellationToken)
        {
            var results = await _repository.GetAllWithTypeAsync();

            return results.Select(r => new ApplicationRequestDto
            {
                Id = r.Id,
                Date = r.Date,
                Status = r.Status,
                Type = r.Type?.Name ?? "Unknown"
            }).ToList();
        }
    }
}
