namespace WaKaDataFetchService
{
    public class WaterSource
    {
        public int Id { get; set; }
        public string IdForUser { get; set; }
        public string Name { get; set; }
        public int SourceType { get; set; }
        public string Connections { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public int Flowrate { get; set; }
        public int NominalDiameter { get; set; }
        public string Notes { get; set; }
        public string Driveway { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string IconUrl { get; set; }
        public IconSize? IconSize { get; set; }
        public IconAnchor? IconAnchor { get; set; }
        public double DistanceInMetres { get; set; }
    }

    public class IconSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class IconAnchor
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class SourceType
    {
        public int Id { get; set; }
        public Dictionary<string, string> Name { get; set; }
    }

    public class WaterData
    {
        public List<WaterSource> WaterSources { get; set; } = [];
        public List<SourceType> SourceTypes { get; set; } = [];
    }
}
