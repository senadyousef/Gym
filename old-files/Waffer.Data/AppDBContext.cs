using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Waffer.Domain.Entities.Identity;
using Waffer.Domain.Entities.Personals;

namespace Waffer.Data
{
    public class AppDBContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public AppDBContext(DbContextOptions options) : base(options) { }

        #region Tables

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Offer> Offers { get; set; }

        #endregion
    }
}
