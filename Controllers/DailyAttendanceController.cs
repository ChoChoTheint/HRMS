using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HRMS.Controllers
{
    public class DailyAttendanceController : Controller
    {
        private readonly HRMSDdContext _dbContent;

        public DailyAttendanceController(HRMSDdContext dbContent)
        {
            _dbContent = dbContent;
        }
        public IActionResult Entry()
        {
            var departments = _dbContent.Department.Select(s => new DepartmentViewModel
            {
                Id = s.Id,
                Code = s.Code
            }).OrderBy(o => o.Code).ToList();
            ViewBag.Departments = departments;

            var employees = _dbContent.Employee.Select(s => new EmployeeViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Id).ToList();
            ViewBag.Employees = employees;
            return View();
        }
        [HttpPost]
        public IActionResult Entry(DailyAttendanceEntity ui)
        {
            try
            {
                DailyAttendanceEntity dailyAttendance = new DailyAttendanceEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    AttendanceDate = ui.AttendanceDate,
                    InTime = ui.InTime,
                    OutTime = ui.OutTime,
                    DepartmentId = ui.DepartmentId,
                    EmployeeId = ui.EmployeeId,
                    Ip = this.GetLocalIPAddress(),
                };
                _dbContent.DailyAttendance.Add(dailyAttendance);
                _dbContent.SaveChanges();
                TempData["info"] = "Save process is completed successfully";
            }
            catch (Exception)
            {

                TempData["info"] = "error occur when save process was done";
            }
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            IList<DailyAttendanceViewModel> dailyAttendances = _dbContent.DailyAttendance.Select(s => new DailyAttendanceViewModel()
            {
                Id = s.Id,
                AttendanceDate = s.AttendanceDate,
                InTime = s.InTime,
                OutTime = s.OutTime,
                DepartmentInfo = _dbContent.Department.Where(d => d.Id == s.DepartmentId).FirstOrDefault().Name,
                EmployeeInfo = _dbContent.Employee.Where(d => d.Id == s.EmployeeId).FirstOrDefault().Name,
             

            }).ToList();
            return View(dailyAttendances);
            
        }
        public IActionResult Delete(string Id)
        {
            try
            {
                DailyAttendanceEntity attendance = _dbContent.DailyAttendance.Where(w => w.Id == Id).FirstOrDefault();
                if (attendance is not null)
                {
                    _dbContent.DailyAttendance.Remove(attendance);
                    _dbContent.SaveChanges();
                    TempData["info"] = "save when deleting the record the system";
                }
                
            }
            catch (Exception ex)
            {
                TempData["info"] = "Error occur when delete process was done.";
            }
            return RedirectToAction("List");
        }
        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
