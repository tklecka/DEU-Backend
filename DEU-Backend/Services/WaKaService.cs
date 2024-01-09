using DEU_Lib.Model;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DEU_Backend.Services
{
    public class WaKaService
    {
        private readonly DeuDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly CustomServiceImplFetcherService _fetcherService;

        public WaKaService(DeuDbContext dbContext, IConfiguration configuration, CustomServiceImplFetcherService fetcherService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _fetcherService = fetcherService;
        }

        public async Task<List<WaKaWaterSource>> FetchAndUpdateWaKaDataAsync()
        {
            CustomServiceImplFetcherService _customServiceImplFetcherService = new CustomServiceImplFetcherService(_configuration);
            var wakaService = _customServiceImplFetcherService.GetWaKaDataFetchService();

            //Get config path from config
            var configPath = _configuration.GetSection("ConfigPaths").GetSection("WaKaDataFetchServiceConfigPath").Value;
            if (configPath == null)
                throw new Exception("Config path for WaKa data fetch service not found");


            var data = await wakaService.FetchDataAsync(configPath, 48.3700241, 14.5150614); //TODO: Get coordinates from config or database

            //Delete old WaKa POIs where SourceWaKaWaterSourceId is not in the new list
            var oldWaKaPOIs = await _dbContext.WaKaWaterSources.Where(p => !data.Select(d => d.SourceWaKaWaterSourceId).Contains(p.SourceWaKaWaterSourceId)).ToListAsync();
            _dbContext.WaKaWaterSources.RemoveRange(oldWaKaPOIs);

            //Add or update WaKa POIs
            foreach (var poi in data)
            {
                var poiInDb = await _dbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.SourceWaKaWaterSourceId == poi.SourceWaKaWaterSourceId);
                if (poiInDb == null)
                {
                    await _dbContext.WaKaWaterSources.AddAsync((WaKaWaterSource)poi);
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

            await _dbContext.SaveChangesAsync();

            return await GetAsync();
        }

        public async Task<List<WaKaWaterSource>> GetAsync(int page = 0, int pageSize = 0)
        {
            if (page <= 0 || pageSize <= 0)
            {
                var data = await _dbContext.WaKaWaterSources.ToListAsync();
                if (data.Count == 0)
                    return [];

                return data;
            }
            else
            {
                var data = await _dbContext.WaKaWaterSources.Skip(page * pageSize).Take(pageSize).ToListAsync();
                if (data.Count == 0)
                    return [];

                return data;
            }
        }

        public async Task<WaKaWaterSource?> GetByIdAsync(int id)
        {
            var data = await _dbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.WaKaWaterSourceId == id);
            if (data == null)
                return null;

            return data;
        }


        public async Task<WaKaWaterSource?> UpdateAsync(int id, WaKaWaterSource poi)
        {
            var poiInDb = await _dbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.WaKaWaterSourceId == id);
            if (poiInDb == null)
                return null;


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

            await _dbContext.SaveChangesAsync();
            return poiInDb;
        }

        public async Task<WaKaWaterSource?> DeleteAsync(int id)
        {
            var poiInDb = await _dbContext.WaKaWaterSources.FirstOrDefaultAsync(p => p.WaKaWaterSourceId == id);
            if (poiInDb == null)
                return null;

            _dbContext.WaKaWaterSources.Remove(poiInDb);
            await _dbContext.SaveChangesAsync();
            return poiInDb;
        }
    }
}
