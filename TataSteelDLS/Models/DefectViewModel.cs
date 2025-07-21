using System.ComponentModel.DataAnnotations;

namespace DefectLoggingApp.Models
{
    public class DefectViewModel
    {
        public CoilData CoilData { get; set; }
        public List<DefectRecord> DefectRecords { get; set; }
        public List<DefectType> DefectTypes { get; set; }
        public int CurrentPosition { get; set; }
        public int CellSize { get; set; }
        public string SearchTerm { get; set; }

        public DefectViewModel()
        {
            CoilData = new CoilData();
            DefectRecords = new List<DefectRecord>();
            DefectTypes = new List<DefectType>();
            CurrentPosition = 0;
            CellSize = 1;
        }
    }
}