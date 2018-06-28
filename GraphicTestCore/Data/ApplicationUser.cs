using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qBI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime LastLogin { get; set; }
        public int AreaId { get; set; }
    }
}
