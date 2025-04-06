using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace reminderApi.Data
{
  public class AppDBContext : IdentityDbContext<AppUser>
  {
    public AppDBContext(DbContextOptions dbContextOptions)
      : base(dbContextOptions) { }

    public DbSet<Reminder> Reminders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      List<IdentityRole> roles =
      [
        new() { Name = "Admin", NormalizedName = "ADMIN" },
        new() { Name = "User", NormalizedName = "USER" },
      ];

      builder.Entity<IdentityRole>().HasData(roles);
    }
  }
}
