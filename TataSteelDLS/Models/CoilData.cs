namespace DefectLoggingApp.Models
{
    public class CoilData
    {
        public int Width { get; set; } = 1175;
        public int Length { get; set; } = 30;
        public int CellHeight { get; set; } = 235; // Height of each cell in pixels
        public string Status { get; set; } = "HOLD";
    }
}