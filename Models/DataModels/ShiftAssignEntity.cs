using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.DataModels
{
    [Table("ShiftAssign")]
    public class ShiftAssignEntity:BaseEntity
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }

        [ForeignKey("ShiftId")]
        public string ShiftId { get; set; }
    }
}
