using Boilerplate.Domain.Entities;
using Boilerplate.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 
         
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemPhotos> ItemPhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TopOfTop> TopOfTop { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<UserEvents> UserEvents { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<PersonalTrainersClasses> PersonalTrainersClasses { get; set; }
        public DbSet<Children> Children { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<UserItems> UserItems { get; set; }
        public DbSet<Carts> Carts { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
