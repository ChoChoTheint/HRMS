using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.DataModels
{
    [Table("Department")]
    public class DepartmentEntity:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ExtensionPhone { get; set; }
        
        //many to one
        public virtual IList<EmployeeEntity> Employees { get; set; }

      
    }
}
