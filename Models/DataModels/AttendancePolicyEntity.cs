using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.DataModels
{
    [Table("AttendancePolicy")]
    public class AttendancePolicyEntity:BaseEntity
    {
        
        public string Name { set; get; }
        public int NumberOfLateTime { set; get; }
        public int NumberOfEarlyOutTime { set; get; }
        public decimal DeductionInAmount { set; get; }
        public int DeductionDay { set; get; } 


    }
    
}
