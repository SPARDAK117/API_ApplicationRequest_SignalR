using Application.Commands.AuthCommands;
using Application.Handlers.AuthHandler;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Handlers.AuthHandlerTests
{
    public class RegisterUserHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenDataIsValid()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            context.Roles.Add(new Role { Id = 1, Name = "User" });
            await context.SaveChangesAsync();

            var handler = new RegisterUserHandler(context);
            var command = new RegisterUserCommand
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "123456",
                RoleId = 1
            };

            var userId = await handler.Handle(command, default);

            var user = await context.LoginCredentials.FindAsync(userId);
            Assert.NotNull(user);
            Assert.Equal("newuser", user.Username);
            Assert.True(BCrypt.Net.BCrypt.Verify("123456", user.PasswordHash));
        }
        [Fact]
        public async Task Handle_ShouldThrow_WhenEmailOrUsernameExists()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            context.LoginCredentials.Add(new LoginCredential
            {
                Username = "existing",
                Email = "existing@example.com",
                PasswordHash = "irrelevant",
                RoleId = 1
            });

            context.Roles.Add(new Role { Id = 1, Name = "User" });

            await context.SaveChangesAsync();

            var handler = new RegisterUserHandler(context);
            var command = new RegisterUserCommand
            {
                Username = "existing",
                Email = "other@example.com",
                Password = "pass",
                RoleId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenRoleDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            var handler = new RegisterUserHandler(context);
            var command = new RegisterUserCommand
            {
                Username = "user",
                Email = "user@example.com",
                Password = "pass",
                RoleId = 99 // no existe
            };

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, default));
        }


    }
}
