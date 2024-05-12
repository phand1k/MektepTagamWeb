using MektepTagamWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MektepTagamWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<CardCode> CardCodes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}