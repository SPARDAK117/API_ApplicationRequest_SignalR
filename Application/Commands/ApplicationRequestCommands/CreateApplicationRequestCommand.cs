using MediatR;

namespace Application.Commands.ApplicationRequestCommands
{
    public record CreateApplicationRequestCommand(int TypeId, string Message) : IRequest<int>;
}
