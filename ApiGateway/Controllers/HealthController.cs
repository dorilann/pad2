using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Health : ControllerBase
    {
        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Is Alive");
        }
    }
}
