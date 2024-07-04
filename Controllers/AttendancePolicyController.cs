using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HRMS.Controllers
{
    public class AttendancePolicyController : Controller
    {
        private readonly HRMSDdContext _dbContext;

        public AttendancePolicyController(HRMSDdContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Entry(AttendancePolicyViewModel attendancePolicyViewModel)
        {
            try
            {
                
                AttendancePolicyEntity attendance = new AttendancePolicyEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = attendancePolicyViewModel.Name,
                    NumberOfLateTime = attendancePolicyViewModel.NumberOfLateTime,
                    NumberOfEarlyOutTime = attendancePolicyViewModel.NumberOfEarlyOutTime,
                    DeductionInAmount = attendancePolicyViewModel.DeductionInAmount,
                    DeductionDay = attendancePolicyViewModel.DeductionDay,

                    Ip = this.GetLocalIPAddress(),
                };
                _dbContext.AttendancePolicy.Add(attendance);
                _dbContext.SaveChanges();
                TempData["info"] = "save successfully the record the system";

            }
            catch (Exception)
            {

                TempData["info"] = "error saving the record the system";
            }
            return RedirectToAction("list");
        }
        public IActionResult List()
        {
            IList<AttendancePolicyViewModel> attendances = _dbContext.AttendancePolicy.Select(s => new AttendancePolicyViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                NumberOfLateTime = s.NumberOfLateTime,
                NumberOfEarlyOutTime = s.NumberOfEarlyOutTime,
                DeductionInAmount = s.DeductionInAmount,
                DeductionDay = s.DeductionDay,
            }).ToList();
            return View(attendances);
        }
        public IActionResult Delete(String Id)
        {
            try
            {
                AttendancePolicyEntity attendance = _dbContext.AttendancePolicy.Where(w => w.Id == Id).FirstOrDefault();
                if (attendance is not null)
                {
                    _dbContext.AttendancePolicy.Remove(attendance);
                    _dbContext.SaveChanges();
                    TempData["info"] = "save when deleting the record the system";
                }
            }
            catch (Exception)
            {
                TempData["info"] = "error when deleting the record the system";
            }
            return RedirectToAction("list");
        }
        public IActionResult Edit(string Id)
        {
            AttendancePolicyViewModel attendance = _dbContext.AttendancePolicy.Where(w => w.Id == Id).Select(s => new AttendancePolicyViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                NumberOfLateTime = s.NumberOfLateTime,
                NumberOfEarlyOutTime = s.NumberOfEarlyOutTime,
                DeductionInAmount = s.DeductionInAmount,
                DeductionDay = s.DeductionDay,
            }).FirstOrDefault();
            return View(attendance);
        }
        [HttpPost]
        public IActionResult Update(AttendancePolicyViewModel attendancePolicyViewModel)
        {
            try
            {

                AttendancePolicyEntity attendance = new AttendancePolicyEntity()
                {
                    Id = attendancePolicyViewModel.Id,
                    Name = attendancePolicyViewModel.Name,
                    NumberOfLateTime = attendancePolicyViewModel.NumberOfLateTime,
                    NumberOfEarlyOutTime = attendancePolicyViewModel.NumberOfEarlyOutTime,
                    DeductionInAmount = attendancePolicyViewModel.DeductionInAmount,
                    DeductionDay = attendancePolicyViewModel.DeductionDay,

                    Ip = this.GetLocalIPAddress(),
                    ModifiedAt = DateTime.Now
                };
                _dbContext.AttendancePolicy.Update(attendance);
                _dbContext.SaveChanges();
                TempData["info"] = "update successfully the record the system";

            }
            catch (Exception)
            {

                TempData["info"] = "error updating the record the system";
            }
            return RedirectToAction("list");
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
