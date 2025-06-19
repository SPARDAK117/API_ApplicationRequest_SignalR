using Application.Commands.ApplicationRequestCommands;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.ApplicationRequestHandlers
{
    public class CreateApplicationRequestHandler : IRequestHandler<CreateApplicationRequestCommand, int>
    {
        private readonly IApplicationRequestRepository _repository;

        public CreateApplicationRequestHandler(IApplicationRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateApplicationRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new ApplicationRequest
            {
                TypeId = request.TypeId,
                Message = request.Message,
                Status = "submitted",
                Date = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return entity.Id;
        }
    }
}
