//using CloudHRMS.Models.DataModels;
//using Ext.Net;
//using HRMS.DAO;
//using HRMS.Models.DataModels;
//using HRMS.Models.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace HRMS.Controllers
//{
//    public class PayrollController : Controller
//    {
//        private readonly HRMSDdContext _dbContext;

//        public PayrollController(HRMSDdContext dbContext)
//        {
//            this._dbContext = dbContext;
//        }
//        public IActionResult Entry()
//        {
//            var departments = _dbContext.Department.Select(s => new DepartmentViewModel
//            {
//                Id = s.Id,
//                Code = s.Code
//            }).OrderBy(o => o.Code).ToList();
//            ViewBag.Departments = departments;

//            var employees = _dbContext.Employee.Select(s => new EmployeeViewModel
//            {
//                Id = s.Id,
//                Name = s.Name
//            }).OrderBy(o => o.Id).ToList();
//            ViewBag.Employees = employees;
//            return View();
//        }
//        [HttpPost]
//        public IActionResult Entry(PayrollEntity ui)
//        {
//            try
//            {
//                List<AttendanceMasterCalculatedData> attendanceMasterCalculatedData = new List<AttendanceMasterCalculatedData>();
//                if (ui.DepartmentId != null)
//                {
//                    //HR,01-03-2024 to 31-03-2024 
//                    List<AttendanceMasterEntity> attendances = _dbContext.AttendanceMaster.Where(w => w.DepartmentId == ui.DepartmentId &&
//                                                                                                                                        (w.AttendanceDate <= ui.ToDate)).OrderBy(o => o.AttendanceDate)
//                                                                                                                                        .ToList();
//                    List<AttendanceMasterEntity> distinctEmployees = attendances.DistinctBy(e => e.EmployeeId).ToList();
//                    foreach (AttendanceMasterEntity distinctEmployee in distinctEmployees)
//                    {
//                        AttendanceMasterCalculatedData calculatedData = new AttendanceMasterCalculatedData();
//                        calculatedData.DepartmentId = distinctEmployee.DepartmentId;
//                        calculatedData.EmployeeId = distinctEmployee.EmployeeId;
//                        calculatedData.FromDate = ui.FromDate;
//                        calculatedData.ToDate = ui.ToDate;
//                        calculatedData.BasicPay = _dbContext.Employee.Find(distinctEmployee.EmployeeId).BasicSalary;
//                        calculatedData.LateCount = attendances.Where(w => w.EmployeeId == distinctEmployee.EmployeeId && w.IsLate == true && (w.AttendanceDate >= ui.FromDate && w.AttendanceDate <= ui.ToDate)).Count();
//                        calculatedData.EarlyOutCount = attendances.Where(w => w.EmployeeId == distinctEmployee.EmployeeId && w.IsEarlyOut == true && (w.AttendanceDate >= ui.FromDate && w.AttendanceDate <= ui.ToDate)).Count();
//                        calculatedData.LeaveCount = attendances.Where(w => w.EmployeeId == distinctEmployee.EmployeeId && w.IsLeave == true && (w.AttendanceDate >= ui.FromDate && w.AttendanceDate <= ui.ToDate)).Count();
//                        calculatedData.AttendanceDays = attendances.Where(w => w.EmployeeId == distinctEmployee.EmployeeId && w.IsLeave == false && (w.AttendanceDate >= ui.FromDate && w.AttendanceDate <= ui.ToDate)).Count();
//                        attendanceMasterCalculatedData.Add(calculatedData);
//                    }
//                    List<PayrollEntity> payrolls = CalculatePayroll(attendanceMasterCalculatedData);
//                    _dbContext.Payroll.AddRange(payrolls);
//                    _dbContext.SaveChanges();
//                    TempData["info"] = "successfully save a record to the system";
//                }
//            }
//            catch (Exception ex)
//            {
//                TempData["info"] = "Error occur when  saving a record  to the system";
//            }
//            return RedirectToAction("list");
//        }
//        private List<PayrollEntity> CalculatePayroll(List<AttendanceMasterCalculatedData> attendanceMasterCalculatedData)
//        {
//            List<PayrollEntity> payrolls = new List<PayrollEntity>();
//            decimal incomeTax = 2000, allowance = 30000, deduction = 10000;
//            int workingDays = 30;
//            foreach (AttendanceMasterCalculatedData calculatedData in attendanceMasterCalculatedData)
//            {
//                PayrollEntity payroll = new PayrollEntity();
//                payroll.Id = Guid.NewGuid().ToString();
//                payroll.CreatedAt = DateTime.Now;
//                payroll.FromDate = calculatedData.FromDate;
//                payroll.ToDate = calculatedData.ToDate;
//                payroll.EmployeeId = calculatedData.EmployeeId;
//                payroll.DepartmentId = calculatedData.DepartmentId;
//                payroll.IncomeTax = incomeTax;
//                payroll.GrossPay = (calculatedData.BasicPay / workingDays) * calculatedData.AttendanceDays + allowance - deduction;
//                payroll.NetPay = payroll.GrossPay - payroll.IncomeTax;
//                payroll.Allowance = allowance;
//                payroll.Deduction = deduction;
//                payroll.AttendanceDays = calculatedData.AttendanceDays;
//                decimal PayPerDay = (calculatedData.BasicPay / workingDays);
//                payroll.AttendanceDeduction = CalculateAttendanceDeductionByAttendancePolicy(calculatedData.FromDate, calculatedData.ToDate, calculatedData.EmployeeId, PayPerDay, calculatedData.LateCount, calculatedData.EarlyOutCount);
//                payrolls.Add(payroll);
//            }
//            return payrolls;
//        }

//        private decimal CalculateAttendanceDeductionByAttendancePolicy(DateTime fromDate, DateTime toDate, string EmployeeId, decimal PayPerDay, int lateCount, int earlyOutCount)
//        {
//            decimal attendanceDeduction = 0;
//            var attendancePolicy = (from attm in _dbContext.AttendanceMaster
//                                    join e in _dbContext.Employee
//                                    on attm.EmployeeId equals e.Id
//                                    join d in _dbContext.Department
//                                    on e.DepartmentId equals d.Id
//                                    join sa in _dbContext.ShiftAssign
//                                    on e.Id equals sa.EmployeeId
//                                    join s in _dbContext.Shift
//                                    on sa.ShiftId equals s.Id
//                                    join ap in _dbContext.AttendancePolicy
//                                    on s.AttendancePolicyId equals ap.Id
//                                    where sa.EmployeeId == EmployeeId && (fromDate >= sa.FromDate && sa.ToDate <= toDate)
//                                    select ap).Distinct();
//            if (null != attendancePolicy)
//            {
//                if (lateCount > attendancePolicy.First().NumberOfLateTime || attendancePolicy.First().NumberOfEarlyOutTime < earlyOutCount)
//                {
//                    attendanceDeduction = attendancePolicy.First().DeductionInAmount ?? 0;
//                }
//                if (attendancePolicy.First().DeductionInDay > 0)
//                {
//                    attendanceDeduction += (PayPerDay * attendancePolicy.First().DeductionInDay) ?? 0;
//                }
//            }
//            return attendanceDeduction;
//        }
//        //public IActionResult List()
//        //{
//        //    List<PayrollViewModel> payrollViews = (from p in _dbContext.Payroll
//        //                                           join e in _dbContext.Employee
//        //                                           on p.EmployeeId equals e.Id
//        //                                           join d in _dbContext.Department
//        //                                           on e.Department equals d.Id
//        //                                           where new PayrollViewModel
//        //                                           {
//        //                                               Id = p.Id,
//        //                                               FromDate = p.FromDate,
//        //                                               ToDate = p.ToDate,
//        //                                               EmployeeId = e.Code+"/"+e.Name,
//        //                                               DepartmentId = d.Code+"/"+d.Name,
//        //                                               GrossPay = p.GrossPay,
//        //                                               NetPay = p.NetPay,
//        //                                               AttendanceDays = p.AttendanceDays,
//        //                                               AttendanceDeduction = p.AttendanceDeduction,
//        //                                               Allowance = p.Allowance,
//        //                                               Deduction = p.Deduction,
//        //                                               IncomeTax = p.IncomeTax,

//        //                                           }).ToList();
//        //    return View("list");
//        //}
//    }
//}
