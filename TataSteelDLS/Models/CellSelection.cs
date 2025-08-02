namespace DefectLoggingApp.Models
{
    public class CellSelection
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public required string DefectType { get; set; }
        public int Severity { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? Remarks { get; set; }
    }
}