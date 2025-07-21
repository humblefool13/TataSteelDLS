using Microsoft.AspNetCore.Mvc;
using DefectLoggingApp.Models;

namespace DefectLoggingApp.Controllers
{
    public class DefectController : Controller
    {
        // In-memory storage for demo purposes
        private static List<DefectRecord> _defectRecords = new List<DefectRecord>();
        private static int _nextId = 1;

        public IActionResult Index()
        {
            var model = new DefectViewModel
            {
                CoilData = new CoilData(),
                DefectRecords = _defectRecords.ToList(),
                DefectTypes = GetDefectTypes(),
                CellSize = 1
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddDefect([FromBody] CellSelection selection)
        {
            try
            {
                var defectType = GetDefectTypes().FirstOrDefault(d => d.Code == selection.DefectType);

                var defect = new DefectRecord
                {
                    Id = _nextId++,
                    StartWidth = selection.StartY,
                    EndWidth = selection.EndY,
                    StartLength = selection.StartX,
                    EndLength = selection.EndX,
                    TotalLength = selection.EndX - selection.StartX + 1,
                    Type = selection.DefectType,
                    DefectName = defectType?.Name ?? selection.DefectType,
                    Severity = selection.Severity
                };

                _defectRecords.Add(defect);

                return Json(new { success = true, defect = defect });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteDefect(int id)
        {
            var defect = _defectRecords.FirstOrDefault(d => d.Id == id);
            if (defect != null)
            {
                _defectRecords.Remove(defect);
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Defect not found" });
        }

        [HttpGet]
        public IActionResult GetDefects()
        {
            return Json(_defectRecords);
        }

        [HttpGet]
        public IActionResult GetGridData(int position = 0, int cellSize = 1)
        {
            var coilData = new CoilData();
            var cells = new List<object>();

            // Calculate visible range based on position
            int startLength = position;
            int endLength = Math.Min(position + 30, coilData.Length);

            for (int x = startLength; x < endLength; x++)
            {
                for (int y = 0; y <= coilData.Width; y += coilData.CellHeight)
                {
                    var cellDefects = _defectRecords.Where(d =>
                        d.StartLength <= x && d.EndLength >= x &&
                        d.StartWidth <= y + coilData.CellHeight && d.EndWidth >= y).ToList();

                    var color = GetCellColor(cellDefects);

                    cells.Add(new
                    {
                        x = x,
                        y = y,
                        color = color,
                        defectCount = cellDefects.Count,
                        hasMultipleDefects = cellDefects.Count > 1
                    });
                }
            }

            return Json(cells);
        }

        private string GetCellColor(List<DefectRecord> defects)
        {
            if (!defects.Any()) return "white";

            if (defects.Count == 1)
            {
                // Single defect colors based on severity/type
                return defects.First().Type switch
                {
                    "SCAB" => "#ff0000", // Red - Indicates Start of a Defect
                    "SCAB-EMBED" => "#ffa500", // Orange - Indicates Start of More Than One Defect
                    "DUSTPIT" => "#ffff00", // Yellow - Endpoint of Defect, Log Name Or Severity
                    "SCUMC-WHIT" => "#90EE90", // Light Green - Cell Having Complete Information of Single Surface Defect
                    "BROWN SHAD" => "#00ff00", // Green - Cell With Multiple Surface Defects, But Complete Information
                    "SLIVER" => "#87CEEB", // Light Blue - Cell With Multiple Shape Defects, But Complete Information
                    _ => "#00ff00"
                };
            }
            else
            {
                // Multiple defects - blue for mixed defects
                return "#0000ff"; // Blue - Cell Having Complete Information, But Mixed Defects
            }
        }

        private List<DefectType> GetDefectTypes()
        {
            return new List<DefectType>
            {
                new DefectType { Code = "SCAB", Name = "Scab" },
                new DefectType { Code = "SCAB-EMBED", Name = "Scab Embedded" },
                new DefectType { Code = "DUSTPIT", Name = "Dust Pit" },
                new DefectType { Code = "SCUMC-WHIT", Name = "Scum White" },
                new DefectType { Code = "BROWN SHAD", Name = "Brown Shadow" },
                new DefectType { Code = "SLIVER", Name = "Sliver" },
                new DefectType { Code = "SLIVER IMP", Name = "Sliver Impression" }
            };
        }

        [HttpPost]
        public IActionResult Search(string searchTerm)
        {
            var filteredDefects = string.IsNullOrEmpty(searchTerm)
                ? _defectRecords.ToList()
                : _defectRecords.Where(d =>
                    d.DefectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            return PartialView("_DefectList", filteredDefects);
        }

        [HttpPost]
        public IActionResult ClearDefects()
        {
            _defectRecords.Clear();
            _nextId = 1;
            return Json(new { success = true });
        }
    }
}