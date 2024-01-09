using DEU_Lib.Model;
using System.Text.Json;

namespace WaKaDataFetchService
{
    public class WaKaDataFetchService : IWaKaDataFetchService
    {
        public async Task<bool> CreateConfig(string Path)
        {
            var config = new WaKaDataFetchServiceConfig();
            return await config.ToJsonFileAsync(Path);
        }

        public async Task<IEnumerable<IWaKaWaterSource>> FetchDataAsync(string configPath, double fireDepartmentLocationLat, double fireDepartmentLocationLng)
        {
            var config = await WaKaDataFetchServiceConfig.FromJsonFileAsync(configPath);
            if (config == null)
            {
                await CreateConfig(configPath);
                throw new Exception("Config file not found. Created new config file. Please fill in the config file and restart the service.");
            }

            var url = $"{config.ApiUrl}&lat={fireDepartmentLocationLat}&lng={fireDepartmentLocationLng}&range={config.Range}&numItems={config.NumItems}&token={config.ApiToken}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"WaKa API returned status code {response.StatusCode} with message: {responseString}");


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var wakaresp = JsonSerializer.Deserialize<WaterData>(responseString, options);

            if (wakaresp == null || wakaresp.WaterSources == null || wakaresp.SourceTypes == null)
                throw new Exception("Could not parse response from WaKa API");

            var wakawatersources = wakaresp?.WaterSources.Select(ws => new WaKaWaterSource
            {
                SourceWaKaWaterSourceId = ws.IdForUser,
                Name = ws.Name,
                Description = ws.Notes,
                Address = ws.Address,
                IconUrl = ws.IconUrl,
                IconWidth = ws.IconSize?.Width ?? 0,
                IconHeight = ws.IconSize?.Height ?? 0,
                IconAnchorX = ws.IconAnchor?.X ?? 0,
                IconAnchorY = ws.IconAnchor?.Y ?? 0,
                Longitude = ws.Longitude,
                Latitude = ws.Latitude,
                Capacity = ws.Capacity,
                Flowrate = ws.Flowrate,
                Connections = ws.Connections,
                //SourceType = wakaresp?.SourceTypes.FirstOrDefault(w => w.Id == ws.SourceType).Name.GetValueOrDefault("de") ?? "",
                SourceType = "Test"
            });

            return wakawatersources ?? throw new Exception("Could not parse response from WaKa API");
        }
    }
}
