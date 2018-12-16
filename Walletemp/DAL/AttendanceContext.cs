using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Walletemp.Models;

namespace Walletemp.DAL
{
    public class AttendanceContext : IdentityDbContext<AppUser>
    {
        public AttendanceContext(DbContextOptions<AttendanceContext>options) : base(options)
        {
            
        }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
