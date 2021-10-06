using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }
        [PersonalData]
        public string Phone { get; set; }
        [PersonalData]
        public string Address1 { get; set; }
        [PersonalData]
        public string Address2 { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string Country { get; set; }
        [PersonalData]
        public string Image { get; set; }
        [PersonalData]
        public bool? IsActive { get; set; }
    }
}
