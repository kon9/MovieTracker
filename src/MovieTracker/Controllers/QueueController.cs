using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Models;

namespace MovieTracker.Controllers;

[Microsoft.AspNetCore.Components.Route("[controller]")]
[ApiController]
[Authorize]

public class QueueController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    
    public QueueController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
}