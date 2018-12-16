using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Walletemp.Models
{
    public class Employee 
    {
        public Employee()
        {
            Attendances = new List<Attendance>();
        }
        public int EmployeeId { get; set; }
        public string Fullname { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
    }
}