using DEU_Backend.Services;
using DEU_Lib.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaKaController(DeuDbContext deuDbContext, IConfiguration configuration) : ControllerBase
    {
        private readonly DeuDbContext _deuDbContext = deuDbContext;
        private readonly IConfiguration _configuration = configuration;

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
            CustomServiceImplFetcherService _customServiceImplFetcherService = new CustomServiceImplFetcherService(_configuration);
            var wakaService = _customServiceImplFetcherService.GetWaKaDataFetchService();

            //Get config path from config
            var configPath = _configuration.GetSection("ConfigPaths").GetSection("WaKaDataFetchServiceConfigPath").Value;
            if (configPath == null)
            {
                return BadRequest("WaKaDataFetchServiceConfigPath not found in appsettings.json");
            }

            var data = await wakaService.FetchDataAsync(configPath, 48.3700241, 14.5150614); //TODO: Get coordinates from config or database

            //Delete old WaKa POIs where SourceWaKaWaterSourceId is not in the new list
            var oldWaKaPOIs = await _deuDbContext.WaKaWaterSources.Where(p => !data.Select(d => d.SourceWaKaWaterSourceId).Contains(p.SourceWaKaWaterSourceId)).ToListAsync();
            _deuDbContext.WaKaWaterSources.RemoveRange(oldWaKaPOIs);

            //Add or update WaKa POIs
            foreach (var poi in data)
            {
                var poiInDb = await _deuDbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.SourceWaKaWaterSourceId == poi.SourceWaKaWaterSourceId);
                if (poiInDb == null)
                {
                    await _deuDbContext.WaKaWaterSources.AddAsync((WaKaWaterSource)poi);
                }
                else
                {
                    poiInDb.Address = poi.Address;
                    poiInDb.Capacity = poi.Capacity;
                    poiInDb.Connections = poi.Connections;
                    poiInDb.Description = poi.Description;
                    poiInDb.Flowrate = poi.Flowrate;
                    poiInDb.IconAnchorX = poi.IconAnchorX;
                    poiInDb.IconAnchorY = poi.IconAnchorY;
                    poiInDb.IconHeight = poi.IconHeight;
                    poiInDb.IconUrl = poi.IconUrl;
                    poiInDb.IconWidth = poi.IconWidth;
                    poiInDb.Latitude = poi.Latitude;
                    poiInDb.Longitude = poi.Longitude;
                    poiInDb.Name = poi.Name;
                    poiInDb.SourceType = poi.SourceType;
                }
            }

            await _deuDbContext.SaveChangesAsync();

            return Ok(data);
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
        [HttpGet("getAllWaKa")]
        public async Task<IActionResult> GetAllWaKaAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 0)
        {
            if (page == 0 || pageSize == 0)
            {
                var data = await _deuDbContext.WaKaWaterSources.ToListAsync();
                if (data.Count == 0)
                {
                    return NotFound("No WaKa POIs found");
                }
                return Ok(data);
            }
            else
            {
                var data = await _deuDbContext.WaKaWaterSources.Skip(page * pageSize).Take(pageSize).ToListAsync();
                if (data.Count == 0)
                {
                    return NotFound("No WaKa POIs found");
                }
                return Ok(data);
            }
        }

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
        [HttpGet("getWaKaById/{id}")]
        public async Task<IActionResult> GetWaKaByIdAsync([FromRoute] int id)
        {
            var data = await _deuDbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.WaKaWaterSourceId == id);
            if (data == null)
            {
                return NotFound("WaKa POI not found");
            }
            return Ok(data);
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
        [HttpPut("updateWaKa/{id}")]
        public async Task<IActionResult> UpdateWaKaAsync([FromRoute] int id, [FromBody] WaKaWaterSource poi)
        {
            var poiInDb = await _deuDbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.WaKaWaterSourceId == id);
            if (poiInDb == null)
            {
                return NotFound("WaKa POI not found");
            }
            poiInDb = poi;
            await _deuDbContext.SaveChangesAsync();
            return Ok(poiInDb);
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
        [HttpDelete("deleteWaKa/{id}")]
        public async Task<IActionResult> DeleteWaKaAsync([FromRoute] int id)
        {
            var poiInDb = await _deuDbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.WaKaWaterSourceId == id);
            if (poiInDb == null)
            {
                return NotFound("WaKa POI not found");
            }
            _deuDbContext.WaKaWaterSources.Remove(poiInDb);
            await _deuDbContext.SaveChangesAsync();
            return Ok(poiInDb);
        }
    }
}
