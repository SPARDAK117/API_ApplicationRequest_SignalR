using Application.Commands.ApplicationRequestCommands;
using Application.Handlers.ApplicationRequestHandlers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Tests.Handlers.ApplicationRequestHandlers
{
    public class DeleteApplicationBatchHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldDeleteEntities_WhenIdsExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(options);

            var request1 = new ApplicationRequest { Id = 33, Message = "test 1", TypeId = 1 };
            var request2 = new ApplicationRequest { Id = 34, Message = "test 2", TypeId = 2 };

            dbContext.ApplicationRequests.AddRange(request1, request2);
            await dbContext.SaveChangesAsync();

            var handler = new DeleteApplicationBatchHandler(dbContext);

            var command = new DeleteApplicationBatchCommand
            {
                Ids = new[] { 33, 34 }
            };

            var result = await handler.Handle(command, default);

            Assert.True(result);
            Assert.Empty(await dbContext.ApplicationRequests.ToListAsync());
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenIdsIsEmpty()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(options);
            var handler = new DeleteApplicationBatchHandler(dbContext);

            var command = new DeleteApplicationBatchCommand
            {
                Ids = Array.Empty<int>()
            };

            var result = await handler.Handle(command, default);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenIdsDoNotExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(options);

            dbContext.ApplicationRequests.Add(new ApplicationRequest { Id = 1, TypeId = 1, Message = "Exists" });
            await dbContext.SaveChangesAsync();

            var handler = new DeleteApplicationBatchHandler(dbContext);

            var command = new DeleteApplicationBatchCommand
            {
                Ids = new[] { 99, 100 }
            };

            var result = await handler.Handle(command, default);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_ShouldDeleteOnlyExisting_WhenSomeIdsExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(options);

            dbContext.ApplicationRequests.Add(new ApplicationRequest { Id = 1, TypeId = 1, Message = "Keep" });
            dbContext.ApplicationRequests.Add(new ApplicationRequest { Id = 2, TypeId = 2, Message = "Delete me" });
            await dbContext.SaveChangesAsync();

            var handler = new DeleteApplicationBatchHandler(dbContext);

            var command = new DeleteApplicationBatchCommand
            {
                Ids = new[] { 2, 99 }
            };

            var result = await handler.Handle(command, default);

            Assert.True(result);
            Assert.Single(dbContext.ApplicationRequests);
            Assert.Equal(1, (await dbContext.ApplicationRequests.FirstAsync()).Id);
        }
    }
}
