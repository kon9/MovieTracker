using Moq;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;
using MovieTracker.Infrastructure.Services;
using MovieTracker.Core.Utilities;
using Xunit;

namespace MovieTracker.Tests
{
    public class AuthenticationTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public AuthenticationTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mapper = TestHelpers.CreateTestMapper();
            _userService = new UserService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task ValidatePasswordAsync_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var hashedPassword = PasswordHasher.HashPassword(password);

            var user = new User
            {
                Id = 1,
                Username = username,
                Email = "test@example.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.GetByUsernameAsync(username))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidatePasswordAsync(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidatePasswordAsync_InvalidUsername_ReturnsFalse()
        {
            // Arrange
            var username = "nonexistent";
            var password = "password123";

            _mockUserRepository.Setup(x => x.GetByUsernameAsync(username))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.ValidatePasswordAsync(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidatePasswordAsync_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            var hashedPassword = PasswordHasher.HashPassword("correctpassword");

            var user = new User
            {
                Id = 1,
                Username = username,
                Email = "test@example.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.GetByUsernameAsync(username))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidatePasswordAsync(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsByUsernameAsync_ExistingUser_ReturnsTrue()
        {
            // Arrange
            var username = "existinguser";
            _mockUserRepository.Setup(x => x.ExistsByUsernameAsync(username))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.ExistsByUsernameAsync(username);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsByUsernameAsync_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            var username = "nonexistent";
            _mockUserRepository.Setup(x => x.ExistsByUsernameAsync(username))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.ExistsByUsernameAsync(username);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsByEmailAsync_ExistingEmail_ReturnsTrue()
        {
            // Arrange
            var email = "existing@example.com";
            _mockUserRepository.Setup(x => x.ExistsByEmailAsync(email))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.ExistsByEmailAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsByEmailAsync_NonExistentEmail_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _mockUserRepository.Setup(x => x.ExistsByEmailAsync(email))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.ExistsByEmailAsync(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_ReturnsUserDto()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password123"
            };

            var user = new User
            {
                Id = 1,
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = PasswordHasher.HashPassword(createUserDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.ExistsByUsernameAsync(createUserDto.Username))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.ExistsByEmailAsync(createUserDto.Email))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.CreateAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessMessage()
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

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.Contains("logged in successfully", result);
        }
    }
}
