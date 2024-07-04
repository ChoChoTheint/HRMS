namespace HRMS.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public DateTime DOB { set; get; }
        public DateTime DOE { set; get; }
        public DateTime? DOR { set; get; }
        public string? Address { set; get; }
        public decimal BasicSalary { set; get; }
        public string? Phone { set; get; }
        public string Gender { set; get; }

        public string DepartmentId { set; get; }
        public string DepartmentInfo{ set; get; }
        public string PositionId { set; get; }
        public string PositionInfo { set; get; }
    }
}
