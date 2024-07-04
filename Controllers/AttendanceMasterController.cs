using Ext.Net;
using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HRMS.Controllers
{
    public class AttendanceMasterController : Controller
    {
        private readonly HRMSDdContext _dbContext;


        public AttendanceMasterController(HRMSDdContext dbContext)
        {
            this._dbContext = dbContext;
        }  
        public IActionResult Entry()
        {
            var departments = _dbContext.Department.Select(s => new DepartmentViewModel
            {
                Id = s.Id,
                Code = s.Code
            }).OrderBy(o => o.Code).ToList();
            ViewBag.Departments = departments;

            var employees = _dbContext.Employee.Select(s => new EmployeeViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name
            }).OrderBy(o => o.Id).ToList();
            ViewBag.Employees = employees;

            var shifts = _dbContext.Shift.Select(s => new ShiftViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Id).ToList();
            ViewBag.Shifts = shifts;
            return View();
        }
        [HttpPost]
        public IActionResult Entry(AttendanceMasterViewModel ui)
        {
            try
            {
                
                //Data exchange from view model to data model by using automapper 
                AttendanceMasterEntity attendanceMasters = new AttendanceMasterEntity();
                
                var DailyAttendancesWithShiftAssignsData = (from d in _dbContext.DailyAttendance
                                                            join sa in _dbContext.ShiftAssign
                                                            on d.EmployeeId equals sa.EmployeeId
                                                            where sa.EmployeeId == ui.EmployeeId &&
                (ui.AttendanceDate >= sa.FromDate && sa.FromDate <= ui.ToDate)
                select new
                                                            {
                                                                dailyAttendance = d,
                                                                shiftAssign = sa
                                                            }).ToList();
                
                foreach (var data in DailyAttendancesWithShiftAssignsData)
                {
                    ShiftEntity definedShift = _dbContext.Shift.Where(s => s.Id == data.shiftAssign.ShiftId).SingleOrDefault();
                    if (definedShift is not null)
                    {
                        AttendanceMasterEntity attendanceMaster = new AttendanceMasterEntity();
                        attendanceMaster.Id = Guid.NewGuid().ToString();
                       // attendanceMaster.CreatedAt = DateTime.Now;
                        attendanceMaster.IsLeave = false;
                        attendanceMaster.InTime = data.dailyAttendance.InTime;
                        attendanceMaster.OutTime = data.dailyAttendance.OutTime;
                        attendanceMaster.EmployeeId = data.shiftAssign.EmployeeId;
                        attendanceMaster.ShiftId = definedShift.Id;
                        attendanceMaster.DepartmentId = data.dailyAttendance.DepartmentId;
                        attendanceMaster.AttendanceDate = data.dailyAttendance.AttendanceDate;
                        attendanceMaster.Ip = this.GetLocalIPAddress();

                        //checking out the late status 
                        if (data.dailyAttendance.InTime > definedShift.LateAfter) //9:16 , 9:15 
                        {
                            attendanceMaster.IsLate = true;
                        }
                        else
                        {
                            attendanceMaster.IsLate = false;
                        }
                        //checking out the late status 
                        if (data.dailyAttendance.OutTime < definedShift.EarlyOutBefore)//18:30 , 17:45
                        {
                            attendanceMaster.IsEarlyOut = true;
                        }
                        else
                        {
                            attendanceMaster.IsEarlyOut = false;
                        }

                    //   attendanceMasters.Add(attendanceMaster);//adding the recrod to the List object  
                    }//end of the deifned shift not null 
                    TempData["info"] = "successfully save a record to the system";
                }
                _dbContext.AttendanceMaster.AddRange(attendanceMasters);//save the recrod to the Db Set <attendance Master> 
                _dbContext.SaveChanges();//saving the data to the database 
                
               
            }
            catch (Exception ex)
            {
                TempData["info"] = "Error occur when  saving a record  to the system";
            }
            return RedirectToAction("list");
        }

        public IActionResult List()
        {
            List<AttendanceMasterViewModel> attendanceMasters = (from attm in _dbContext.AttendanceMaster
                                                                 join e in _dbContext.Employee
                                                                 on attm.EmployeeId equals e.Id
                                                                 join d in _dbContext.Department
                                                                 on e.DepartmentId equals d.Id
                                                                 join sa in _dbContext.ShiftAssign
                                                                 on e.Id equals sa.EmployeeId
                                                                 join s in _dbContext.Shift
                                                                 on sa.ShiftId equals s.Id
                                                                 select new AttendanceMasterViewModel
                                                                 {
                                                                     Id = attm.Id,
                                                                     AttendanceDate = attm.AttendanceDate,
                                                                     InTime = attm.InTime,
                                                                     OutTime = attm.OutTime,
                                                                     ShiftInfo = s.Name,
                                                                     EmployeeInfo = e.Code + "/" + e.Name,
                                                                     DepartmentInfo = d.Code + "/" + d.Name,
                                                                 }).ToList();
            return View(attendanceMasters);

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
