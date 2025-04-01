using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
  // Learning info: we are using IdentityDbContext with the AppUser type to gain access to built-in Identity properties 
  // such as UserName, Email, etc.
  // Without Identity, we would typically inherit from just DbContext.
  public class DataContext : IdentityDbContext<AppUser>
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFollowing> UserFollowings { get; set; }

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
      .HasOne(u => u.Activity)
      .WithMany(a => a.Attendees)
      .HasForeignKey(aa => aa.ActivityId);

      // We will add configuration when activity is deleted that also all comments associated with that activity will be deleted.
      builder.Entity<Comment>()
      .HasOne(a => a.Activity)
      .WithMany(c => c.Comments)
      .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<UserFollowing>(b =>
      {
        b.HasKey(k => new { k.ObserverId, k.TargetId });

        b.HasOne(o => o.Observer)
          .WithMany(f => f.Followings)
          .HasForeignKey(o => o.ObserverId)
          .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(o => o.Target)
          .WithMany(f => f.Followers)
          .HasForeignKey(o => o.TargetId)
          .OnDelete(DeleteBehavior.Cascade);
      });

    }

  }
}