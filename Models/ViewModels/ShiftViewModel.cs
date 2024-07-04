using HRMS.Models.DataModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.ViewModels
{
    public class ShiftViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public TimeSpan LateAfter { get; set; }
        public TimeSpan EarlyOutBefore { get; set; }


        public string AttendancePolicyId { set; get; }
        public string AttendancePolicyInfo { set; get; }

    }
}
