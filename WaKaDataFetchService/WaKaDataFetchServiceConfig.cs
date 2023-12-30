
namespace WaKaDataFetchService
{
    internal class WaKaDataFetchServiceConfig
    {

        public string ApiUrl { get; set; } = "https://api.wasserkarte.info/2.0/getSurroundingWaterSources/?source=wakaapp";
        public string ApiToken { get; set; } = "TODO: Insert API Token here";

        public int Range { get; set; } = 1000;

        public int NumItems { get; set; } = 1000;

        public string ToJsonString() => System.Text.Json.JsonSerializer.Serialize(this);

        public async Task<bool> ToJsonFileAsync(string path)
        {
            var configExists = File.Exists(path);
            if (!configExists)
            {
                await File.WriteAllTextAsync(path, ToJsonString());
                return true;
            }
            return false;
        }

        public static WaKaDataFetchServiceConfig? FromJsonString(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<WaKaDataFetchServiceConfig>(json);
        }

        public static async Task<WaKaDataFetchServiceConfig?> FromJsonFileAsync(string path)
        {
            return FromJsonString(await File.ReadAllTextAsync(path));
        }
    }
}
