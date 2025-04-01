using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
  public class DataContext : IdentityDbContext<AppUser>
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
    public DbSet<Photo> Photos { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // Configure composite primary key for the join table ActivityAttendee
      // This ensures that each AppUser can only attend each Activity once
      builder.Entity<ActivityAttendee>(x =>
          x.HasKey(aa => new { aa.AppUserId, aa.ActivityId }));

      // Configure relationship: one AppUser can attend many Activities
      builder.Entity<ActivityAttendee>()
        .HasOne(aa => aa.AppUser)
        .WithMany(u => u.Activities)
        .HasForeignKey(aa => aa.AppUserId);

      // Configure relationship: one Activity can have many Attendees
      builder.Entity<ActivityAttendee>()
        .HasOne(aa => aa.Activity)
        .WithMany(a => a.Attendees)
        .HasForeignKey(aa => aa.ActivityId);
    }

  }
}