namespace HRMS.Models.ViewModels
{
    public class DailyAttendanceViewModel
    {
        public string Id { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }

        public string DepartmentId { set; get; }
        public string DepartmentInfo { set; get; }

        public string EmployeeId { set; get; }

        public string EmployeeInfo { set; get; }
    }
}
