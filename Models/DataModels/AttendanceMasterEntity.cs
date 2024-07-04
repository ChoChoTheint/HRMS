using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.DataModels
{
    [Table("AttendanceMaster")]
    public class AttendanceMasterEntity:BaseEntity
    {
        public DateTime AttendanceDate { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public Boolean IsLate { get; set; }
        public Boolean IsEarlyOut { get; set; }
        public Boolean IsLeave { get; set; }

        [ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }

        [ForeignKey("DepartmentId")]
        public string DepartmentId { get; set; }

        [ForeignKey("ShiftId")]
        public string ShiftId { get; set; }


    }
}
