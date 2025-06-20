using Application.Handlers.ApplicationRequestHandlers;
using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Tests.Handlers.ApplicationRequestHandlers
{
    public class GetAllApplicationRequestsHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnMappedDtos_WhenEntitiesExist()
        {
            // Arrange
            var mockRepo = new Mock<IApplicationRequestRepository>();
            mockRepo.Setup(r => r.GetAllWithTypeAsync()).ReturnsAsync(new List<ApplicationRequest>
            {
                new ApplicationRequest
                {
                    Id = 1,
                    Status = "completed",
                    Date = DateTime.UtcNow,
                    Type = new RequestType { Name = "Request" }
                }
            });

            var handler = new GetAllApplicationRequestsHandler(mockRepo.Object);
            var query = new GetAllApplicationRequestsQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.Single(result);
            Assert.Equal("Request", result[0].Type);
        }
        [Fact]
        public async Task Handle_ShouldReturnUnknownType_WhenTypeIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IApplicationRequestRepository>();
            mockRepo.Setup(r => r.GetAllWithTypeAsync()).ReturnsAsync(new List<ApplicationRequest>
    {
        new ApplicationRequest
        {
            Id = 2,
            Status = "submitted",
            Date = DateTime.UtcNow,
            Type = null // simula Type null
        }
    });

            var handler = new GetAllApplicationRequestsHandler(mockRepo.Object);
            var query = new GetAllApplicationRequestsQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.Single(result);
            Assert.Equal("Unknown", result[0].Type);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoEntitiesExist()
        {
            var mockRepo = new Mock<IApplicationRequestRepository>();
            mockRepo.Setup(r => r.GetAllWithTypeAsync()).ReturnsAsync(new List<ApplicationRequest>());

            var handler = new GetAllApplicationRequestsHandler(mockRepo.Object);
            var query = new GetAllApplicationRequestsQuery();

            var result = await handler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

    }
}
