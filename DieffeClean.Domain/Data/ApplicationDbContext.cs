using DieffeClean.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DieffeClean.Domain.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<MyUser> MyUsers { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<UserApartment> UserApartments { get; set; }
    
    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}