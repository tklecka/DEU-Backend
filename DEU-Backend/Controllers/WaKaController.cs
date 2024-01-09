using DEU_Backend.Services;
using DEU_Lib.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaKaController(WaKaService waKaService) : ControllerBase
    {
        /// <summary>
        /// Fetch WaKa POIs from WaKa service and store/update them in the database and remove old POIs from the database that are not in the new list.
        /// </summary>
        /// <returns>
        /// List of WaKa POIs
        /// </returns>
        /// <response code="200">Returns the list of WaKa POIs</response>
        [HttpGet("fetchWaKaFromService")]
        public async Task<IActionResult> FetchWaKaFromServiceAsync()
        {
            try
            {
                var resp = await waKaService.FetchAndUpdateWaKaDataAsync();
                return Ok(resp);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get all WaKa POIs from the database with pagination. Returns all WaKa POIs if page and pageSize are not set.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>
        /// List of WaKa POIs
        /// </returns>
        /// <response code="200">Returns the list of WaKa POIs</response>
        /// <response code="404">No WaKa POIs found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 0) => Ok(await waKaService.GetAsync(page, pageSize));

        /// <summary>
        /// Get a WaKa POI by its ID
        /// </summary>
        /// <param name="id">ID of the WaKa POI</param>
        /// <returns>
        /// WaKa POI
        /// </returns>
        /// <response code="200">Returns the WaKa POI</response>
        /// <response code="404">WaKa POI not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var poi = await waKaService.GetByIdAsync(id);
            if (poi == null)
            {
                return NotFound("WaKa POI not found");
            }
            return Ok(poi);
        }

        /// <summary>
        /// Update a WaKa POI
        /// </summary>
        /// <param name="id">ID of the WaKa POI</param>
        /// <param name="poi">Updated WaKa POI</param>
        /// <returns>
        /// Updated WaKa POI
        /// </returns>
        /// <response code="200">Returns the updated WaKa POI</response>
        /// <response code="404">WaKa POI not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] WaKaWaterSource poi)
        {
            var poiResult = await waKaService.UpdateAsync(id, poi);
            if (poiResult == null)
            {
                return NotFound("WaKa POI not found");
            }
            return Ok(poiResult);
        }

        /// <summary>
        /// Delete a WaKa POI
        /// </summary>
        /// <param name="id">ID of the WaKa POI</param>
        /// <returns>
        /// Deleted WaKa POI
        /// </returns>
        /// <response code="200">Returns the deleted WaKa POI</response>
        /// <response code="404">WaKa POI not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var poi = await waKaService.DeleteAsync(id);
            if (poi == null)
            {
                return NotFound("WaKa POI not found");
            }
            return Ok(poi);
        }
    }
}
