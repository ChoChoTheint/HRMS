using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HRMS.Controllers
{
    public class ShiftController : Controller
    {
        private readonly HRMSDdContext _dbContent;
        public ShiftController(HRMSDdContext dbContext)
        {
            _dbContent = dbContext;
        }
        public IActionResult Entry()
        {
            var policy = _dbContent.AttendancePolicy.Select(s => new AttendancePolicyViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Id).ToList();
            ViewBag.AttendancePolicy = policy;
            return View();
        }
        [HttpPost]
        public IActionResult Entry(ShiftViewModel ui)
        {
            try
            {
                ShiftEntity shift = new ShiftEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ui.Name,
                    InTime = ui.InTime,
                    OutTime = ui.OutTime,
                    LateAfter = ui.LateAfter,
                    EarlyOutBefore = ui.EarlyOutBefore,
                    AttendancePolicyId = ui.AttendancePolicyId,
                    Ip = this.GetLocalIPAddress(),
                };
                _dbContent.Shift.Add(shift);
                _dbContent.SaveChanges();
                TempData["info"] = "Save process is completed successfully";
            }
            catch (Exception)
            {

                TempData["info"] = "error process is occur";
            }
            return View("List");
        }
        public IActionResult List()
        {
            IList<ShiftViewModel> shift = _dbContent.Shift.Select(s => new ShiftViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                InTime = s.InTime,
                OutTime = s.OutTime,
                LateAfter = s.LateAfter,
                EarlyOutBefore = s.EarlyOutBefore,
                AttendancePolicyInfo = _dbContent.AttendancePolicy.Where(d => d.Id == s.AttendancePolicyId).FirstOrDefault().Name,
            }).ToList();
            return View(shift);
        }
        public IActionResult Delete(string Id)
        {
            try
            {
                var shift = _dbContent.Shift.Find(Id);
                if (shift is not null)
                {
                    _dbContent.Remove(shift);
                    _dbContent.SaveChanges();
                }
                TempData["info"] = "Delete process is completed successfully.";
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
