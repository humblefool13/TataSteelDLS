using DefectLoggingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace DefectLoggingApp.Models
{
    public class DefectRecord
    {
        public int Id { get; set; }

        [Display(Name = "St Wd")]
        public int StartWidth { get; set; }

        [Display(Name = "End Wd")]
        public int EndWidth { get; set; }

        [Display(Name = "St Ln")]
        public int StartLength { get; set; }

        [Display(Name = "End Ln")]
        public int EndLength { get; set; }

        [Display(Name = "Tot Ln")]
        public int TotalLength { get; set; }

        public required string Type { get; set; }

        [Display(Name = "Defect Name")]
        public required string DefectName { get; set; }

        public int Severity { get; set; }

        [Display(Name = "Defect Image")]
        public string? DefectImage { get; set; }

        public string? MotherCoil { get; set; }
        public string? DaughterCoil { get; set; }
        public string? Remarks { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}