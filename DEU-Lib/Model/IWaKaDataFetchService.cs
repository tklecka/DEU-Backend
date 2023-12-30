namespace DEU_Lib.Model
{
    public interface IWaKaDataFetchService
    {
        Task<bool> CreateConfig(string Path);
        Task<IEnumerable<IWaKaWaterSource>> FetchDataAsync(string configPath, double fireDepartmentLocationLat, double fireDepartmentLocationLng);
    }
}
