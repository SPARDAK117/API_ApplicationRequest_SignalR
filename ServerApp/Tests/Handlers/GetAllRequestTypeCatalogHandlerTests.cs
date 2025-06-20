using Application.Handlers.CatalogHandler;
using Application.Queries.CatalogsQueries;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Handlers.CatalogHandlerTests
{
    public class GetAllRequestTypeCatalogHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnCatalogDtos_WhenRequestTypesExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            context.RequestTypes.AddRange(
                new RequestType { Id = 1, Name = "Complaint" },
                new RequestType { Id = 2, Name = "Offer" }
            );
            await context.SaveChangesAsync();

            var handler = new GetAllRequestTypeCatalogHandler(context);
            var query = new GetRequestTypesQuery();

            var result = await handler.Handle(query, default);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == "Complaint");
            Assert.Contains(result, r => r.Name == "Offer");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoRequestTypesExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var handler = new GetAllRequestTypeCatalogHandler(context);
            var query = new GetRequestTypesQuery();

            var result = await handler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Empty(result);
        }


    }
}
