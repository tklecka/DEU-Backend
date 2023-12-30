using Microsoft.AspNetCore.Mvc;

namespace DEU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <returns>
        /// test endpoint
        /// </returns>
        /// <response code="200">Test</response>
        [HttpGet("test")]
        public ActionResult Test()
        {
            return Ok("Test");
        }
    }
}
