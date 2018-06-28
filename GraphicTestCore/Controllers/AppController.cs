using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace qBIPro.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        
        public IActionResult Data()
        {
            return View();
        }

        [IgnoreAntiforgeryTokenAttribute]
        public async Task<ActionResult> OnPost()
        {

            IFormFile file = Request.Form.Files[0];
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            FileInfo fileinfo = new FileInfo(filePath);
            StringBuilder sb = new StringBuilder();
            using (ExcelPackage package = new ExcelPackage(fileinfo))
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                DataTable excel = GetDataTableFromExcel(worksheet, true);
                string json = JsonConvert.SerializeObject(excel, Formatting.Indented);

                return Content(ConvertDataTableToHTML(excel));
                //return Json(json);
            }
        }

        private static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table class='table table-striped table-responsive'>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        public static DataTable GetDataTableFromExcel(ExcelWorksheet ws, bool hasHeaderRow = true)
        {
            var tbl = new DataTable();
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                tbl.Columns.Add(hasHeaderRow ?
                    firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            var startRow = hasHeaderRow ? 2 : 1;
            for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                var row = tbl.NewRow();
                foreach (var cell in wsRow)
                {
                        row[cell.Start.Column - 1] = cell.Text;
                }

                tbl.Rows.Add(row);
            }
            return tbl;
        }

        public IActionResult Preview()
        {
            return View();
        }

        public IActionResult Graphs()
        {
            return View();
        }

        public IActionResult Maps()
        {
            return View();
        }
    }
}