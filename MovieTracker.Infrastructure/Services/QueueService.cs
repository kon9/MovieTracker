using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;

namespace MovieTracker.Infrastructure.Services
{
    public class QueueService : IQueueService
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IQueueItemRepository _queueItemRepository;
        private readonly IQueueMemberRepository _queueMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public QueueService(
            IQueueRepository queueRepository,
            IQueueItemRepository queueItemRepository,
            IQueueMemberRepository queueMemberRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _queueRepository = queueRepository;
            _queueItemRepository = queueItemRepository;
            _queueMemberRepository = queueMemberRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<QueueDto?> GetByIdAsync(int id)
        {
            var queue = await _queueRepository.GetByIdAsync(id);
            if (queue == null) return null;

            var items = await _queueItemRepository.GetByQueueIdAsync(id);
            var members = await _queueMemberRepository.GetByQueueIdAsync(id);

            var queueDto = _mapper.Map<QueueDto>(queue);
            queueDto.Items = _mapper.Map<List<QueueItemDto>>(items);
            queueDto.Members = _mapper.Map<List<QueueMemberDto>>(members);
            queueDto.TotalItems = items.Count();
            queueDto.CompletedItems = items.Count(i => i.Status == QueueItemStatus.Completed);

            return queueDto;
        }

        public async Task<IEnumerable<QueueDto>> GetAllAsync()
        {
            var queues = await _queueRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<QueueDto>>(queues);
        }

        public async Task<IEnumerable<QueueDto>> GetByOwnerIdAsync(int ownerId)
        {
            var queues = await _queueRepository.GetByOwnerIdAsync(ownerId);
            return _mapper.Map<IEnumerable<QueueDto>>(queues);
        }

        public async Task<IEnumerable<QueueDto>> GetPublicQueuesAsync()
        {
            var queues = await _queueRepository.GetPublicQueuesAsync();
            return _mapper.Map<IEnumerable<QueueDto>>(queues);
        }

        public async Task<IEnumerable<QueueDto>> GetByMemberIdAsync(int memberId)
        {
            var queues = await _queueRepository.GetByMemberIdAsync(memberId);
            return _mapper.Map<IEnumerable<QueueDto>>(queues);
        }

        public async Task<QueueDto> CreateAsync(int ownerId, CreateQueueDto createQueueDto)
        {
            var owner = await _userRepository.GetByIdAsync(ownerId);
            if (owner == null)
                throw new InvalidOperationException("Owner not found");

            var queue = _mapper.Map<Queue>(createQueueDto);
            queue.OwnerId = ownerId;
            queue.CreatedAt = DateTime.UtcNow;

            var createdQueue = await _queueRepository.CreateAsync(queue);

            // Add owner as first member
            var ownerMember = new QueueMember
            {
                QueueId = createdQueue.Id,
                UserId = ownerId,
                Role = QueueMemberRole.Owner,
                JoinedAt = DateTime.UtcNow
            };

            await _queueMemberRepository.CreateAsync(ownerMember);

            return await GetByIdAsync(createdQueue.Id) ?? _mapper.Map<QueueDto>(createdQueue);
        }

        public async Task<QueueDto> UpdateAsync(int id, int userId, UpdateQueueDto updateQueueDto)
        {
            var queue = await _queueRepository.GetByIdAsync(id);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsOwnerAsync(id, userId))
                throw new InvalidOperationException("Only the owner can update the queue");

            if (!string.IsNullOrEmpty(updateQueueDto.Name))
                queue.Name = updateQueueDto.Name;
            if (updateQueueDto.Description != null)
                queue.Description = updateQueueDto.Description;
            if (updateQueueDto.IsPublic.HasValue)
                queue.IsPublic = updateQueueDto.IsPublic.Value;

            queue.UpdatedAt = DateTime.UtcNow;

            var updatedQueue = await _queueRepository.UpdateAsync(queue);
            return _mapper.Map<QueueDto>(updatedQueue);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            if (!await _queueRepository.ExistsAsync(id))
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsOwnerAsync(id, userId))
                throw new InvalidOperationException("Only the owner can delete the queue");

            await _queueRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _queueRepository.ExistsAsync(id);
        }

        public async Task<bool> IsMemberAsync(int queueId, int userId)
        {
            return await _queueRepository.IsMemberAsync(queueId, userId);
        }

        public async Task<bool> IsOwnerAsync(int queueId, int userId)
        {
            return await _queueRepository.IsOwnerAsync(queueId, userId);
        }

        public async Task<QueueDto> AddItemToQueueAsync(int queueId, int userId, AddItemToQueueDto addItemDto)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsMemberAsync(queueId, userId))
                throw new InvalidOperationException("You are not a member of this queue");

            var position = addItemDto.Position ?? await _queueItemRepository.GetNextPositionAsync(queueId);

            var queueItem = new QueueItem
            {
                QueueId = queueId,
                Title = addItemDto.Title,
                Description = addItemDto.Description,
                Type = addItemDto.Type,
                ExternalId = addItemDto.ExternalId,
                ImageUrl = addItemDto.ImageUrl,
                Position = position,
                Status = QueueItemStatus.Pending,
                AddedAt = DateTime.UtcNow,
                AddedById = userId
            };

            await _queueItemRepository.CreateAsync(queueItem);

            return await GetByIdAsync(queueId) ?? _mapper.Map<QueueDto>(queue);
        }

        public async Task<QueueDto> UpdateQueueItemAsync(int queueId, int itemId, int userId, UpdateQueueItemDto updateItemDto)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsMemberAsync(queueId, userId))
                throw new InvalidOperationException("You are not a member of this queue");

            var item = await _queueItemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new InvalidOperationException("Queue item not found");

            if (!string.IsNullOrEmpty(updateItemDto.Status))
            {
                if (Enum.TryParse<QueueItemStatus>(updateItemDto.Status, out var status))
                {
                    item.Status = status;
                    if (status == QueueItemStatus.Completed)
                        item.CompletedAt = DateTime.UtcNow;
                }
            }

            if (updateItemDto.Position.HasValue)
                item.Position = updateItemDto.Position.Value;

            if (!string.IsNullOrEmpty(updateItemDto.Title))
                item.Title = updateItemDto.Title;

            if (updateItemDto.Description != null)
                item.Description = updateItemDto.Description;

            await _queueItemRepository.UpdateAsync(item);

            return await GetByIdAsync(queueId) ?? _mapper.Map<QueueDto>(queue);
        }

        public async Task<QueueDto> RemoveItemFromQueueAsync(int queueId, int itemId, int userId)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsMemberAsync(queueId, userId))
                throw new InvalidOperationException("You are not a member of this queue");

            var item = await _queueItemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new InvalidOperationException("Queue item not found");

            await _queueItemRepository.DeleteAsync(itemId);

            return await GetByIdAsync(queueId) ?? _mapper.Map<QueueDto>(queue);
        }

        public async Task<QueueDto> AddMemberToQueueAsync(int queueId, int ownerId, AddMemberToQueueDto addMemberDto)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsOwnerAsync(queueId, ownerId))
                throw new InvalidOperationException("Only the owner can add members");

            var user = await _userRepository.GetByIdAsync(addMemberDto.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            if (await _queueMemberRepository.ExistsByQueueAndUserAsync(queueId, addMemberDto.UserId))
                throw new InvalidOperationException("User is already a member of this queue");

            var member = new QueueMember
            {
                QueueId = queueId,
                UserId = addMemberDto.UserId,
                Role = Enum.Parse<QueueMemberRole>(addMemberDto.Role),
                JoinedAt = DateTime.UtcNow
            };

            await _queueMemberRepository.CreateAsync(member);

            return await GetByIdAsync(queueId) ?? _mapper.Map<QueueDto>(queue);
        }

        public async Task<QueueDto> RemoveMemberFromQueueAsync(int queueId, int memberId, int userId)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsOwnerAsync(queueId, userId))
                throw new InvalidOperationException("Only the owner can remove members");

            var member = await _queueMemberRepository.GetByQueueAndUserIdAsync(queueId, memberId);
            if (member == null)
                throw new InvalidOperationException("User is not a member of this queue");

            await _queueMemberRepository.DeleteAsync(member.Id);

            return await GetByIdAsync(queueId) ?? _mapper.Map<QueueDto>(queue);
        }

        public async Task<QueueDto> UpdateMemberRoleAsync(int queueId, int memberId, int userId, string newRole)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);
            if (queue == null)
                throw new InvalidOperationException("Queue not found");

            if (!await _queueRepository.IsOwnerAsync(queueId, userId))
                throw new InvalidOperationException("Only the owner can update member roles");

            var member = await _queueMemberRepository.GetByQueueAndUserIdAsync(queueId, memberId);
            if (member == null)
                throw new InvalidOperationException("User is not a member of this queue");

            if (!Enum.TryParse<QueueMemberRole>(newRole, out var role))
                throw new InvalidOperationException("Invalid role");

            member.Role = role;
            await _queueMemberRepository.UpdateAsync(member);

            return await GetByIdAsync(queueId) ?? _mapper.Map<QueueDto>(queue);
        }
    }
} 