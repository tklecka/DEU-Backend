namespace DEU_Lib.Model
{
    public interface IDepartment
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string? ShortName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Operation> Operations { get; set; }

    }

    public class Department : IDepartment
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}
