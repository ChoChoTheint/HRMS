using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HRMS.Controllers
{
    public class ShiftAssignController : Controller
    {
        private readonly HRMSDdContext _dbContext;

        public ShiftAssignController(HRMSDdContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Entry()
        {
            var shift = _dbContext.Shift.Select(s => new ShiftViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Id).ToList();
            ViewBag.Shifts = shift;

            var employees = _dbContext.Employee.Select(s => new EmployeeViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Id).ToList();
            ViewBag.Employees = employees;
            return View();
        }
        [HttpPost]
        public IActionResult Entry(ShiftAssignViewModel ui)
        {
            try
            {
                ShiftAssignEntity shiftAssign = new ShiftAssignEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromDate = ui.FromDate,
                    ToDate = ui.ToDate,
                    ShiftId = ui.ShiftId,
                    EmployeeId = ui.EmployeeId,
                    Ip = this.GetLocalIPAddress(),
                };
                _dbContext.ShiftAssign.Add(shiftAssign);
                _dbContext.SaveChanges();
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
            IList<ShiftAssignViewModel> shiftAssign = _dbContext.ShiftAssign.Select(s => new ShiftAssignViewModel()
            {
                Id = s.Id,
                FromDate = s.FromDate,
                ToDate = s.ToDate,
                ShiftInfo = _dbContext.Shift.Where(d => d.Id == s.ShiftId).FirstOrDefault().Name,
                EmployeeInfo = _dbContext.Employee.Where(d => d.Id == s.EmployeeId).FirstOrDefault().Name,


            }).ToList();
            return View(shiftAssign);
        }
        public IActionResult Delete(string Id)
        {
            try
            {
                ShiftAssignEntity shiftAssign = _dbContext.ShiftAssign.Where(w => w.Id == Id).FirstOrDefault();
                if (shiftAssign is not null)
                {
                    _dbContext.ShiftAssign.Remove(shiftAssign);
                    _dbContext.SaveChanges();
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
