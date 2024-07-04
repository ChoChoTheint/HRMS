using HRMS.Models.ViewModels;
using HRMS.Reports;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class EmployeeReportController : Controller
    {
        private readonly IEmployeeReport _employeeReport;
        public EmployeeReportController(IEmployeeReport employeeReport)
        {
            _employeeReport = employeeReport;
        }
        public IActionResult EmployeeDetailReport() => View();

        //public IActionResult EmployeeReportFromCodeToCode(string fromCode, string toCode)
        //{
        //    string reportName = $"EmployeeDetail_{Guid.NewGuid():N}.xlsx";

        //    IList<EmployeeDetailReportViewModel> employees = _employeeReport.EmployeeDetailReport(fromCode, toCode);



        //    //if (employees.Any())
        //    //{
        //    //    var exportData = FilesIOHelper.ExporttoExcel<EmployeeDetailReportViewModel>(employees, "employeeDetailReport");
        //    //    return File(exportData, "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet", reportName);
        //    //}
        //    //else
        //    //{
        //    //    TempData["info"] = "There no data when report to excel";
        //    //    return View();
        //    //}
        //}
    }
}
