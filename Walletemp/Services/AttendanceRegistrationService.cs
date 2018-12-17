using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Walletemp.DAL;
using Walletemp.Models;

namespace Walletemp.Services
{
    public class AttendanceRegistrationService : IAttendanceRegistrationService
    {
        private readonly AttendanceContext _context;

        public AttendanceRegistrationService(AttendanceContext context)
        {
            _context = context;
        }

        //Mark registration for today
        public async Task RegistrationForToday(string userId)
        {
            var user = await _context.Employees.FirstAsync(e => e.UserId == userId);

            //Check if it is weekend
            if (IsWeekend())
            {
                //Check the attendance closing time
                var settings = await _context.Settings.FirstAsync();

                if (DateTime.Now < settings.AttendanceTime)
                {
                    //Check if there is an existing record for the day
                    var anyAttendance = await _context.Attendances.AnyAsync(e =>
                        e.EmployeeId == user.EmployeeId && e.AttendanceTime.ToShortDateString() == DateTime.Today.ToShortDateString());

                    if (!anyAttendance)
                    {
                        //record attendance
                        var newAtten = new Attendance
                        {
                            EmployeeId = user.EmployeeId,
                            AttendanceTime = DateTime.UtcNow
                        };

                        _context.Attendances.Add(newAtten);
                        await _context.SaveChangesAsync();
                    }
                   
                }
            }
        }

        public bool IsWeekend()
        {
            DayOfWeek today = DateTime.Today.DayOfWeek;

            if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
                return false;

            return true;
        }

        public async Task<IQueryable<Attendance>> AttendanceByDayTask(int employeeId)
        {
            throw new NotImplementedException();
        }
    }

    public interface IAttendanceRegistrationService
    {
        //Registration for a day
        Task RegistrationForToday(string userId);

        //if its weekend
        bool IsWeekend();

        Task<IQueryable<Attendance>> AttendanceByDayTask(int employeeId);

    }
}
