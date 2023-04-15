using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YIN_Portal.Models;


namespace YIN_Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRoles, string>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options)
        {
        }


        public DbSet<Testing> Testings { get; set; }

    }
}
