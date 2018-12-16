using System;

namespace Walletemp.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime AttendanceTime { get; set; }
    }
}