namespace DefectLoggingApp.Models
{
    public class GridCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; } = "white";
        public List<DefectRecord> Defects { get; set; } = new List<DefectRecord>();
        public bool IsSelected { get; set; }
    }
}