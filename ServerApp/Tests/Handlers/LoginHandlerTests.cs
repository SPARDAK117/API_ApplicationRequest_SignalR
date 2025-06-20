using Application.Commands.AuthCommands;
using Application.Handlers.AuthHandler;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Handlers.AuthHandlerTests
{
    public class LoginHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAuthResult_WhenCredentialsAreValid()
        {
            // Arrange
            var input = "user@example.com";
            var password = "Password123";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var mockRepo = new Mock<ILoginCredentialRepository>();
            mockRepo.Setup(r => r.GetByUsernameOrEmailAsync(input)).ReturnsAsync(new LoginCredential
            {
                Id = 1,
                Username = "user1",
                PasswordHash = passwordHash,
                Role = new Role { Name = "Admin" }
            });

            var mockJwt = new Mock<IJwtService>();
            mockJwt.Setup(j => j.GenerateToken(1, "user1", "Admin")).Returns("fake-jwt");

            var handler = new LoginHandler(mockRepo.Object, mockJwt.Object);
            var command = new LoginCommand { Input = input, Password = password };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1", result.Username);
            Assert.Equal("Admin", result.Role);
            Assert.Equal("fake-jwt", result.Token);
        }
        [Fact]
        public async Task Handle_ShouldThrowUnauthorized_WhenUserNotFound()
        {
            var mockRepo = new Mock<ILoginCredentialRepository>();
            mockRepo.Setup(r => r.GetByUsernameOrEmailAsync("no-user")).ReturnsAsync((LoginCredential?)null);

            var mockJwt = new Mock<IJwtService>();
            var handler = new LoginHandler(mockRepo.Object, mockJwt.Object);

            var command = new LoginCommand { Input = "no-user", Password = "irrelevant" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorized_WhenPasswordIsInvalid()
        {
            var mockRepo = new Mock<ILoginCredentialRepository>();
            mockRepo.Setup(r => r.GetByUsernameOrEmailAsync("user")).ReturnsAsync(new LoginCredential
            {
                Id = 1,
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
                Role = new Role { Name = "User" }
            });

            var mockJwt = new Mock<IJwtService>();
            var handler = new LoginHandler(mockRepo.Object, mockJwt.Object);

            var command = new LoginCommand { Input = "user", Password = "WrongPassword" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, default));
        }
        [Fact]
        public async Task Handle_ShouldThrowInvalidOperation_WhenUserHasNoRole()
        {
            var password = "Secret123";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var mockRepo = new Mock<ILoginCredentialRepository>();
            mockRepo.Setup(r => r.GetByUsernameOrEmailAsync("user")).ReturnsAsync(new LoginCredential
            {
                Id = 1,
                Username = "user",
                PasswordHash = passwordHash,
                Role = null
            });

            var mockJwt = new Mock<IJwtService>();
            var handler = new LoginHandler(mockRepo.Object, mockJwt.Object);

            var command = new LoginCommand { Input = "user", Password = password };

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, default));
        }


    }
}
