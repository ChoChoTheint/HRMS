using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HRMS.Models.DataModels
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt {get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public bool IsInActive { get; set; }
        public string Ip { get; set; }


    }
}
