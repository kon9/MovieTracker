using Microsoft.AspNetCore.Mvc;

namespace MovieTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<T> HandleResult<T>(T result)
        {
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        protected ActionResult<T> HandleResult<T>(T? result, string notFoundMessage = "Resource not found")
        {
            if (result == null)
                return NotFound(notFoundMessage);

            return Ok(result);
        }

        protected ActionResult HandleException(Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
} 