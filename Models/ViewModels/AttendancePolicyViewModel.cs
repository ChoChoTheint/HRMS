namespace HRMS.Models.ViewModels
{
    public class AttendancePolicyViewModel
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public int NumberOfLateTime { set; get; }
        public int NumberOfEarlyOutTime { set; get; }
        public decimal DeductionInAmount { set; get; }
        public int DeductionDay { set; get; }
    }
}
