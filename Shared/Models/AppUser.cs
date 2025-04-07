using Microsoft.AspNetCore.Identity;

namespace Shared.Models
{
  public class AppUser : IdentityUser
  {
    public ICollection<Reminder> Reminders { get; set; } = [];
  }
}
