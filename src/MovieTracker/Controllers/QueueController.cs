using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Infrastructure.Interfaces.Service;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Controllers;

[Microsoft.AspNetCore.Components.Route("[controller]")]
[ApiController]
[Authorize]

public class QueueController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IQueueService _queueService;
    
    public QueueController(UserManager<AppUser> userManager, IQueueService queueService)
    {
        _userManager = userManager;
        _queueService = queueService;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QueueVm>> GetQueue(int queueId)
    {
        var queueVm = await _queueService.GetQueueAsync(queueId);
        return Ok(queueVm);
    }
    
    [HttpPost()]
    public async Task<ActionResult<QueueVm>> CreateQueue(QueueDto queueDto)
    {
        var queueVm = await _queueService.CreateQueueAsync(queueDto);
        return CreatedAtAction(nameof(GetQueue), new { id = queueVm.Id }, queueVm);
    }

}