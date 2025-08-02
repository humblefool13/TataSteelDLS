using System.ComponentModel.DataAnnotations;

namespace DefectLoggingApp.Models
{
    public class DefectViewModel
    {
        public required CoilData CoilData { get; set; }
        public required List<DefectRecord> DefectRecords { get; set; }
        public required List<DefectType> DefectTypes { get; set; }
        public int CellSize { get; set; }
    }
}