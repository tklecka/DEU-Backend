using DEU_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

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

        [HttpGet("getWaka")]
        public async Task<ActionResult> GetWaKaFromCustomServiceAsync()
        {
            CustomServiceImplFetcherService _customServiceImplFetcherService = new CustomServiceImplFetcherService(_configuration);
            var wakaService = _customServiceImplFetcherService.GetWaKaDataFetchService();
            await wakaService.CreateConfig("./test.json");
            var data = await wakaService.FetchDataAsync("./test.json", 48.3700241, 14.5150614);
            return Ok(data);
        }
    }
}
