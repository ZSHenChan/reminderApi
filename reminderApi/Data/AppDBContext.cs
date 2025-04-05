using System;
using Microsoft.EntityFrameworkCore;
using personal_ai.Models;

namespace personal_ai.Data
{
  public class AppDBContext : DbContext
  {
    public AppDBContext(DbContextOptions dbContextOptions)
      : base(dbContextOptions) { }

    public DbSet<Reminder> Reminders { get; set; }
  }
}
