using HRMS.DAO;
using HRMS.Models.ViewModels;
using HRMS.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace HRMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly HRMSDdContext _dbContext;

        public DepartmentController(HRMSDdContext dbContext)
        {
            _dbContext = dbContext;
        }

        public HRMSDdContext DbContext { get; }

        public IActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Entry(DepartmentViewModel departmentViewModel)
        {
            try
            {
                DepartmentEntity department = new DepartmentEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = departmentViewModel.Code,
                    Name = departmentViewModel.Name,
                    ExtensionPhone = departmentViewModel.ExtensionPhone,

                    Ip = this.GetLocalIPAddress(),
                };
                _dbContext.Department.Add(department);
                _dbContext.SaveChanges();
                TempData["info"] = "save successfully the record the system";
            }
            catch (Exception)
            {

                TempData["info"] = "error successfully the record the system";
            }
            return RedirectToAction("list");
        }

        public IActionResult List()
        {
            IList<DepartmentViewModel> departments = _dbContext.Department.Select(s => new DepartmentViewModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                ExtensionPhone = s.ExtensionPhone,
                TotalEmployeeCount = s.Employees.Count
            }).ToList();
            return View(departments);
        }

        public IActionResult Delete(string Id)
        {
            
            try
            {
                DepartmentEntity department = _dbContext.Department.Where(w => w.Id == Id).FirstOrDefault();
                if (department is not null)
                {
                    _dbContext.Department.Remove(department);
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
            DepartmentViewModel department = _dbContext.Department.Where(w => w.Id == Id).Select(s => new DepartmentViewModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                ExtensionPhone = s.ExtensionPhone,
            }).FirstOrDefault();
            return View(department);
        }
        [HttpPost]
        public IActionResult Update(DepartmentViewModel departmentViewModel)
        {
            try
            {
                DepartmentEntity department = new DepartmentEntity()
                {
                    Id = departmentViewModel.Id,
                    Code = departmentViewModel.Code,
                    Name = departmentViewModel.Name,
                    ExtensionPhone = departmentViewModel.ExtensionPhone,

                    Ip = this.GetLocalIPAddress(),
                    ModifiedAt = DateTime.Now
                };
                _dbContext.Department.Update(department);
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
        public IActionResult ExportToExcel(string htmlTable)
        {
            if (htmlTable == null)
            {
                // Handle the case where htmlTable is null
                // You could return an error response or throw an exception
                // For demonstration, let's return a BadRequest result
                return BadRequest("HTML table is null.");
            }

            // Proceed with converting htmlTable to bytes and returning the file
            return File(Encoding.ASCII.GetBytes(htmlTable), "application/vnd.ms-excel", "htmltable.xls");
        }

    }
}
