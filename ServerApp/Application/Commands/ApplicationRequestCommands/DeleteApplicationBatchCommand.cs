using MediatR;

namespace Application.Commands.ApplicationRequestCommands
{
    public class DeleteApplicationBatchCommand : IRequest<bool>
    {
        public int[] Ids { get; set; } = [];
    }
}
