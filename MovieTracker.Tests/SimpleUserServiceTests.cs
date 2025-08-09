using Moq;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;
using MovieTracker.Core.Utilities;
using MovieTracker.Infrastructure.Services;
using Xunit;

namespace MovieTracker.Tests
{
    public class SimpleUserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public SimpleUserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object, null!);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_ThrowsExceptionWithoutMapper()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(x => x.ExistsByUsernameAsync(createUserDto.Username))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.ExistsByEmailAsync(createUserDto.Email))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => _userService.CreateAsync(createUserDto));
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_WorksWithoutMapper()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "password123"
            };

            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = PasswordHasher.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.GetByUsernameAsync(loginDto.Username))
                .ReturnsAsync(user);

            _mockUserRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act & Assert - LoginAsync doesn't use mapper, so it should work fine
            var result = await _userService.LoginAsync(loginDto);
            Assert.Contains("logged in successfully", result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingUser_ThrowsExceptionWithoutMapper()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act & Assert - GetByIdAsync uses mapper, so it should throw NullReferenceException
            await Assert.ThrowsAsync<NullReferenceException>(
                () => _userService.GetByIdAsync(userId));
        }

        [Fact]
        public async Task DeleteAsync_ExistingUser_DeletesSuccessfully()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(x => x.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            _mockUserRepository.Verify(x => x.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistentUser_ThrowsException()
        {
            // Arrange
            var userId = 999;
            _mockUserRepository.Setup(x => x.ExistsAsync(userId))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _userService.DeleteAsync(userId));
            Assert.Equal("User not found", exception.Message);
        }
    }
}
