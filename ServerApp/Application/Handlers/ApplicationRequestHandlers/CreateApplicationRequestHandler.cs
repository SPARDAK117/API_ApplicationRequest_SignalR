using Application.Commands.ApplicationRequestCommands;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.ApplicationRequestHandlers
{
    public class CreateApplicationRequestHandler(IApplicationRequestRepository repository) : IRequestHandler<CreateApplicationRequestCommand, int>
    {
        private readonly IApplicationRequestRepository _repository = repository;

        public async Task<int> Handle(CreateApplicationRequestCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException("Message cannot be empty");
            if (request.TypeId <= 0)
                throw new ArgumentException("Invalid TypeId");

            ApplicationRequest entity = new()
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
