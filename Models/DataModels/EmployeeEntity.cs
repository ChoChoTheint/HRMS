using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.DataModels
{
    [Table("Employee")]
    public class EmployeeEntity:BaseEntity
    {
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

        //one-to-many relationship between employee and department
        [ForeignKey("DepartmentId")]
        public string DepartmentId { set; get; }
        public virtual DepartmentEntity Department { get; set; }

        //one-to-many relationship between employee and position
        [ForeignKey("PositionId")]
        public string PositionId { set; get;}
        public virtual PositionEntity Position { get; set; }

    }
}
