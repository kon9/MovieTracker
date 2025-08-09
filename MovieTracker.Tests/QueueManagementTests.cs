using Moq;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;
using MovieTracker.Infrastructure.Services;
using Xunit;

namespace MovieTracker.Tests
{
    public class QueueManagementTests
    {
        private readonly Mock<IQueueRepository> _mockQueueRepository;
        private readonly Mock<IQueueItemRepository> _mockQueueItemRepository;
        private readonly Mock<IQueueMemberRepository> _mockQueueMemberRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly QueueService _queueService;

        public QueueManagementTests()
        {
            _mockQueueRepository = new Mock<IQueueRepository>();
            _mockQueueItemRepository = new Mock<IQueueItemRepository>();
            _mockQueueMemberRepository = new Mock<IQueueMemberRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            var mapper = TestHelpers.CreateTestMapper();
            _queueService = new QueueService(
                _mockQueueRepository.Object,
                _mockQueueItemRepository.Object,
                _mockQueueMemberRepository.Object,
                _mockUserRepository.Object,
                mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingQueue_ReturnsQueue()
        {
            // Arrange
            var queueId = 1;
            var queue = new Queue
            {
                Id = queueId,
                Name = "My Watchlist",
                Description = "Movies I want to watch",
                IsPublic = false,
                OwnerId = 1,
                CreatedAt = DateTime.UtcNow
            };

            _mockQueueRepository.Setup(x => x.GetByIdAsync(queueId))
                .ReturnsAsync(queue);
            _mockQueueItemRepository.Setup(x => x.GetByQueueIdAsync(queueId))
                .ReturnsAsync(new List<QueueItem>());
            _mockQueueMemberRepository.Setup(x => x.GetByQueueIdAsync(queueId))
                .ReturnsAsync(new List<QueueMember>());

            // Act
            var result = await _queueService.GetByIdAsync(queueId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(queue.Id, result.Id);
            Assert.Equal(queue.Name, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistentQueue_ReturnsNull()
        {
            // Arrange
            var queueId = 999;
            _mockQueueRepository.Setup(x => x.GetByIdAsync(queueId))
                .ReturnsAsync((Queue?)null);

            // Act
            var result = await _queueService.GetByIdAsync(queueId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPublicQueues_ReturnsPublicQueues()
        {
            // Arrange
            var publicQueues = new List<Queue>
            {
                new Queue { Id = 1, Name = "Public Queue 1", IsPublic = true, OwnerId = 1 },
                new Queue { Id = 2, Name = "Public Queue 2", IsPublic = true, OwnerId = 2 }
            };

            _mockQueueRepository.Setup(x => x.GetPublicQueuesAsync())
                .ReturnsAsync(publicQueues);

            // Act
            var result = await _queueService.GetPublicQueuesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, queue => Assert.True(queue.IsPublic));
        }

        [Fact]
        public async Task GetByOwnerIdAsync_ReturnsUserQueues()
        {
            // Arrange
            var userId = 1;
            var userQueues = new List<Queue>
            {
                new Queue { Id = 1, Name = "User Queue 1", OwnerId = userId },
                new Queue { Id = 2, Name = "User Queue 2", OwnerId = userId }
            };

            _mockQueueRepository.Setup(x => x.GetByOwnerIdAsync(userId))
                .ReturnsAsync(userQueues);

            // Act
            var result = await _queueService.GetByOwnerIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, queue => Assert.Equal(userId, queue.OwnerId));
        }

        [Fact]
        public async Task CreateAsync_ValidQueue_ReturnsQueueDto()
        {
            // Arrange
            var ownerId = 1;
            var createQueueDto = new CreateQueueDto
            {
                Name = "My Watchlist",
                Description = "Movies I want to watch",
                IsPublic = false
            };

            var owner = new User
            {
                Id = ownerId,
                Username = "testuser",
                Email = "test@example.com"
            };

            var queue = new Queue
            {
                Id = 1,
                Name = createQueueDto.Name,
                Description = createQueueDto.Description,
                IsPublic = createQueueDto.IsPublic,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.GetByIdAsync(ownerId))
                .ReturnsAsync(owner);
            _mockQueueRepository.Setup(x => x.CreateAsync(It.IsAny<Queue>()))
                .ReturnsAsync(queue);
            _mockQueueMemberRepository.Setup(x => x.CreateAsync(It.IsAny<QueueMember>()))
                .ReturnsAsync(new QueueMember());

            // Act
            var result = await _queueService.CreateAsync(ownerId, createQueueDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(queue.Id, result.Id);
            Assert.Equal(queue.Name, result.Name);
            Assert.Equal(queue.Description, result.Description);
            Assert.Equal(queue.IsPublic, result.IsPublic);
        }

        [Fact]
        public async Task CreateAsync_InvalidOwnerId_ThrowsException()
        {
            // Arrange
            var ownerId = 999;
            var createQueueDto = new CreateQueueDto
            {
                Name = "My Watchlist",
                Description = "Movies I want to watch",
                IsPublic = false
            };

            _mockUserRepository.Setup(x => x.GetByIdAsync(ownerId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _queueService.CreateAsync(ownerId, createQueueDto));
            Assert.Equal("Owner not found", exception.Message);
        }

        [Fact]
        public async Task ExistsAsync_ExistingQueue_ReturnsTrue()
        {
            // Arrange
            var queueId = 1;
            _mockQueueRepository.Setup(x => x.ExistsAsync(queueId))
                .ReturnsAsync(true);

            // Act
            var result = await _queueService.ExistsAsync(queueId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_NonExistentQueue_ReturnsFalse()
        {
            // Arrange
            var queueId = 999;
            _mockQueueRepository.Setup(x => x.ExistsAsync(queueId))
                .ReturnsAsync(false);

            // Act
            var result = await _queueService.ExistsAsync(queueId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsMemberAsync_UserIsMember_ReturnsTrue()
        {
            // Arrange
            var queueId = 1;
            var userId = 1;
            _mockQueueRepository.Setup(x => x.IsMemberAsync(queueId, userId))
                .ReturnsAsync(true);

            // Act
            var result = await _queueService.IsMemberAsync(queueId, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsOwnerAsync_UserIsOwner_ReturnsTrue()
        {
            // Arrange
            var queueId = 1;
            var userId = 1;
            _mockQueueRepository.Setup(x => x.IsOwnerAsync(queueId, userId))
                .ReturnsAsync(true);

            // Act
            var result = await _queueService.IsOwnerAsync(queueId, userId);

            // Assert
            Assert.True(result);
        }
    }
}
