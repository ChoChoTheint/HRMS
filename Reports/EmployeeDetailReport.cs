using HRMS.DAO;
using HRMS.Models.ViewModels;

namespace HRMS.Reports
{
    public class EmployeeDetailReport
    {
        private readonly HRMSDdContext _dbContext;

        public EmployeeDetailReport(HRMSDdContext dbContext)
        {
            _dbContext = dbContext;
        }


        //IList<EmployeeDetailReportViewModel> IEmployeeReport.EmployeeDetailReport(string fromCode, string toCode)
        //{
        //    IList<EmployeeDetailReportViewModel> employees = (from e in _dbContext.Employee
        //                                                      join d in _dbContext.Department
        //                                                      on e.DepartmentId equals d.Id
        //                                                      join p in _dbContext.Position
        //                                                      on e.PositionId equals p.Id
        //                                                      where e.Code.CompareTo(fromCode) >= 0 && e.Code.CompareTo(toCode) <= 0

        //                                                      select new EmployeeDetailReportViewModel
        //                                                      {
        //                                                          Name = e.Name,
        //                                                          Email = e.Email,
        //                                                          DOB = e.DOB,
        //                                                          BasicSalary = e.BasicSalary,
        //                                                          Address = e.Address,
        //                                                          Gender = e.Gender,
        //                                                        //  Phone = e.Phone,
        //                                                          Code = e.Code,
        //                                                          DOE = e.DOE,
        //                                                          DepartmentInfo = d.Code + "/" + d.Name,
        //                                                          PositionInfo = p.Code + "/" + p.Name,

        //                                                      }).ToList();
        //    return employees;


        //}
    }
}
