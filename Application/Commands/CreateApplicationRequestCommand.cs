using MediatR;

namespace Application.Commands
{
    public record CreateApplicationRequestCommand(int TypeId, string Message) : IRequest<int>;
}
