
using Microsoft.EntityFrameworkCore;
using HRMS.Models.DataModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace HRMS.DAO
{	
    public class HRMSDdContext: IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public HRMSDdContext(DbContextOptions<HRMSDdContext> dbOptions) : base(dbOptions)
        {

        }
        // register the domain class(entites) as DBSets
        public DbSet<PositionEntity> Position { get; set; }

        public DbSet<DepartmentEntity> Department { get; set; }

        public DbSet<AttendancePolicyEntity> AttendancePolicy { get; set; }
        public DbSet<EmployeeEntity> Employee { get; set; }
        public DbSet<DailyAttendanceEntity> DailyAttendance { get; set; }
        public DbSet<ShiftEntity> Shift { get;set ; }

        public DbSet<ShiftAssignEntity> ShiftAssign { get; set; }

        public DbSet<AttendanceMasterEntity> AttendanceMaster { get; set; }
        public DbSet<PayrollEntity> Payroll { get; set; }

    }
}
