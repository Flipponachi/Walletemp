using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Walletemp.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
