namespace DEU_Lib.Model
{
    public interface IWaKaWaterSource
    {
        int WaKaWaterSourceId { get; set; }

        /// <summary>
        /// Source ID from the WaKa Provider
        /// </summary>
        string SourceWaKaWaterSourceId { get; set; }
        string Name { get; set; }

        /// <summary>
        /// Source type (e.g. "Hydrant")
        /// </summary>
        string SourceType { get; set; }

        /// <summary>
        /// Description/Info of the water source
        /// </summary>
        string? Description { get; set; }
        string? Address { get; set; }
        string? IconUrl { get; set; }
        double IconWidth { get; set; }
        double IconHeight { get; set; }
        double IconAnchorX { get; set; }
        double IconAnchorY { get; set; }
        double Longitude { get; set; }
        double Latitude { get; set; }

        /// <summary>
        /// Capacity in cubic meters
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// Flowrate in liters per minute
        /// </summary>
        int Flowrate { get; set; }

        /// <summary>
        /// Outlets for hoses (e.g. 2x C, 1x B, 1x A)
        /// </summary>
        string? Connections { get; set; }
    }

    public class WaKaWaterSource : IWaKaWaterSource
    {
        public int WaKaWaterSourceId { get; set; }
        public string SourceWaKaWaterSourceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SourceType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? IconUrl { get; set; }
        public double IconWidth { get; set; }
        public double IconHeight { get; set; }
        public double IconAnchorX { get; set; }
        public double IconAnchorY { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Capacity { get; set; }
        public int Flowrate { get; set; }
        public string? Connections { get; set; }
    }
}
