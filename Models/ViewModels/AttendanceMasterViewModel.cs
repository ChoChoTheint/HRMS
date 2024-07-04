using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.ViewModels
{
    public class AttendanceMasterViewModel
    {
        public string Id { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public Boolean IsLate { get; set; }
        public Boolean IsEarlyOut { get; set; }
        public Boolean IsLeave { get; set; }

        public string DepartmentId { set; get; }
        public string DepartmentInfo { set; get; }

        public string EmployeeId { set; get; }
        public string EmployeeInfo { set; get; }
        public string ShiftId { set; get; }
        public string ShiftInfo { set; get; }
    }
}
