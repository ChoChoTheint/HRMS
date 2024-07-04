using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HRMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HRMSDdContext _dbContent;

        public EmployeeController(HRMSDdContext dbContent)
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

            var positions = _dbContent.Position.Select(s => new PositionViewModel
            {
                Id = s.Id,
                Code = s.Code
            }).OrderBy(o => o.Code).ToList();
            ViewBag.Positions = positions;
            return View();
        }
        [HttpPost]
        public IActionResult Entry(EmployeeViewModel ui)
        {
            try
            {
           
                EmployeeEntity employee = new EmployeeEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = ui.Code,
                    Name = ui.Name,
                    Email = ui.Email,
                    Phone = ui.Phone,
                    DOB = ui.DOB,
                    DOE = ui.DOE,
                    DOR = ui.DOR,
                    Address = ui.Address,
                    BasicSalary = ui.BasicSalary,
                    Gender = ui.Gender,
                    DepartmentId = ui.DepartmentId,
                    PositionId = ui.PositionId,
                    Ip = this.GetLocalIPAddress(),

                };
                _dbContent.Employee.Add(employee);
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
            IList<EmployeeViewModel> employees = _dbContent.Employee.Select(s => new EmployeeViewModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                DOB = s.DOB,
                DOE = s.DOE,
                DOR = s.DOR,
                Address = s.Address,
                BasicSalary = s.BasicSalary,
                Gender = s.Gender,
                DepartmentInfo = _dbContent.Department.Where(d => d.Id == s.DepartmentId).FirstOrDefault().Name,
                PositionInfo = _dbContent.Position.Where(d => d.Id == s.PositionId).FirstOrDefault().Name,

            }).ToList();
            return View(employees);
        }

        public IActionResult Edit(string Id)
        {
            var departments = _dbContent.Department.Select(s => new DepartmentViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Name).ToList();
           

            var positions = _dbContent.Position.Select(s => new PositionViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Name).ToList();


            EmployeeViewModel employee = _dbContent.Employee.Where(x=>x.Id == Id).Select(s => new EmployeeViewModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                DOB = s.DOB,
                DOE = s.DOE,
                DOR = s.DOR,
                Address = s.Address,
                BasicSalary = s.BasicSalary,
                Gender = s.Gender,
                DepartmentId = s.DepartmentId,
                PositionId = s.PositionId,
            }).FirstOrDefault();

            ViewBag.Departments = departments;
            ViewBag.Positions = positions;
            return View(employee);
        }
        [HttpPost]
        public IActionResult Update(EmployeeEntity ui)
        {
            try
            {
                EmployeeEntity employee = new EmployeeEntity()
                {
                    Id = ui.Id,
                    Code = ui.Code,
                    Name = ui.Name,
                    Email = ui.Email,
                    Phone = ui.Phone,
                    DOB = ui.DOB,
                    DOE = ui.DOE,
                    DOR = ui.DOR,
                    Address = ui.Address,
                    BasicSalary = ui.BasicSalary,
                    Gender = ui.Gender,
                    ModifiedAt = DateTime.Now,

                    DepartmentId = ui.DepartmentId,
                    PositionId = ui.PositionId,
                };
                _dbContent.Employee.Update(employee);
                _dbContent.SaveChanges();
                TempData["info"] = "Save process is completed successfully";
            }
            catch (Exception)
            {
                TempData["info"] = "error process is completed successfully";
            }
            return RedirectToAction("List");

        }
        public IActionResult Delete(string Id)
        {
            try
            {
                var employee = _dbContent.Employee.Find(Id);
                if (employee is not null)
                {
                    _dbContent.Remove(employee);
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
