using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YIN_Portal.Models
{
    public class ApplicationRoles:IdentityRole
    {
        public int ParentBranch { get; set; }
    }
}
