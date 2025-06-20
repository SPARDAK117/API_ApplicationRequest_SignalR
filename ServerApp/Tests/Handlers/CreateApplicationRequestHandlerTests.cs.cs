using Application.Commands.ApplicationRequestCommands;
using Application.Handlers.ApplicationRequestHandlers;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Tests.Handlers.ApplicationRequestHandlers
{
    public class CreateApplicationRequestHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldAddApplicationRequest_AndReturnId()
        {
            var mockRepo = new Mock<IApplicationRequestRepository>();

            mockRepo.Setup(r => r.AddAsync(It.IsAny<ApplicationRequest>()))
                .Callback<ApplicationRequest>(a => a.Id = 123)
                .Returns(Task.CompletedTask);

            mockRepo.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            CreateApplicationRequestHandler handler = new CreateApplicationRequestHandler(mockRepo.Object);

            var command = new CreateApplicationRequestCommand(1, "Test request");

            var result = await handler.Handle(command, default);

            Assert.Equal(123, result);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ApplicationRequest>()), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenMessageIsEmpty()
        {
            var mockRepo = new Mock<IApplicationRequestRepository>();
            var handler = new CreateApplicationRequestHandler(mockRepo.Object);

            var command = new CreateApplicationRequestCommand(1, "");

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenTypeIdIsZeroOrNegative()
        {
            var mockRepo = new Mock<IApplicationRequestRepository>();
            var handler = new CreateApplicationRequestHandler(mockRepo.Object);
            var command = new CreateApplicationRequestCommand(0, "Valid message");

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, default));
        }
    }
}
