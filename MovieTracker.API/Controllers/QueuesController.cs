using Microsoft.AspNetCore.Mvc;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Services;

namespace MovieTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueuesController : BaseApiController
    {
        private readonly IQueueService _queueService;

        public QueuesController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queues = await _queueService.GetAllAsync();
            return Ok(queues);
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicQueues()
        {
            var queues = await _queueService.GetPublicQueuesAsync();
            return Ok(queues);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyQueues()
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queues = await _queueService.GetByOwnerIdAsync(userId);
            return Ok(queues);
        }

        [HttpGet("member")]
        public async Task<IActionResult> GetMemberQueues()
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queues = await _queueService.GetByMemberIdAsync(userId);
            return Ok(queues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var queue = await _queueService.GetByIdAsync(id);
            if (queue == null)
                return NotFound();

            return Ok(queue);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQueueDto createQueueDto)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.CreateAsync(userId, createQueueDto);
            return CreatedAtAction(nameof(GetById), new { id = queue.Id }, queue);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateQueueDto updateQueueDto)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.UpdateAsync(id, userId, updateQueueDto);
            return Ok(queue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: Get from JWT token
            int userId = 1;
            await _queueService.DeleteAsync(id, userId);
            return NoContent();
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(int id, AddItemToQueueDto addItemDto)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.AddItemToQueueAsync(id, userId, addItemDto);
            return Ok(queue);
        }

        [HttpPut("{id}/items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int id, int itemId, UpdateQueueItemDto updateItemDto)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.UpdateQueueItemAsync(id, itemId, userId, updateItemDto);
            return Ok(queue);
        }

        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int id, int itemId)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.RemoveItemFromQueueAsync(id, itemId, userId);
            return Ok(queue);
        }

        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddMember(int id, AddMemberToQueueDto addMemberDto)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.AddMemberToQueueAsync(id, userId, addMemberDto);
            return Ok(queue);
        }

        [HttpDelete("{id}/members/{memberId}")]
        public async Task<IActionResult> RemoveMember(int id, int memberId)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.RemoveMemberFromQueueAsync(id, memberId, userId);
            return Ok(queue);
        }

        [HttpPut("{id}/members/{memberId}/role")]
        public async Task<IActionResult> UpdateMemberRole(int id, int memberId, [FromBody] string newRole)
        {
            // TODO: Get from JWT token
            int userId = 1;
            var queue = await _queueService.UpdateMemberRoleAsync(id, memberId, userId, newRole);
            return Ok(queue);
        }
    }
} 