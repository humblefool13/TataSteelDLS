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
            if (selection == null)
                return Json(new { success = false, error = "Body is empty" });

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
                    Severity = selection.Severity,
                    Timestamp = selection.Timestamp,
                    Remarks = selection.Remarks ?? string.Empty,
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
        public IActionResult DeleteDefect([FromBody] int id)
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
            int endLength = Math.Min(position + (30*cellSize), coilData.Length);


            int cellHeight = (int)Math.Ceiling(coilData.Width / 5.0);

            for (int x = startLength; x < endLength; x += cellSize)
            {
                for (int y = 0; y <= coilData.Width; y += cellHeight)
                {
                    var cellDefects = _defectRecords.Where(d =>
                        d.StartLength <= x && d.EndLength >= x &&
                        d.StartWidth < y && d.EndWidth >= y).ToList();

                    var color = GetCellColor(cellDefects);

                    cells.Add(new
                    {
                        x,
                        y,
                        color,
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
                return "#00ff00";
            }
            else
            {
                return "#38761d";
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
                new DefectType { Code = "BROWN-SHAD", Name = "Brown Shadow" },
                new DefectType { Code = "SLIVER", Name = "Sliver" },
                new DefectType { Code = "SLIVER-IMP", Name = "Sliver Impression" }
            };
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