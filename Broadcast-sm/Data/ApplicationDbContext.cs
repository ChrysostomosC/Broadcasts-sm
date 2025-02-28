using Broadcast_sm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Broadcast_sm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Broadcast> Broadcasts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure one-to-many relationship for Broadcasts
            builder.Entity<Broadcast>()
                   .HasOne(b => b.User)
                   .WithMany(u => u.Broadcasts)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure the self-referencing many-to-many for ListeningTo (existing)
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.ListeningTo)
                .WithMany() // or use .WithMany(u => u.Followers) if you have the inverse navigation property
                .UsingEntity<Dictionary<string, object>>(
                    "UserListening",
                    j => j.HasOne<ApplicationUser>()
                          .WithMany()
                          .HasForeignKey("ListeningToId")
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<ApplicationUser>()
                          .WithMany()
                          .HasForeignKey("UserId")
                          .OnDelete(DeleteBehavior.Cascade)
                );

            // Configure many-to-many relationship for Broadcast Likes
            builder.Entity<Broadcast>()
                .HasMany(b => b.Likes)
                .WithMany() // No reciprocal navigation property on ApplicationUser
                .UsingEntity<Dictionary<string, object>>(
                    "BroadcastLikes", // Name of the join table
                    j => j.HasOne<ApplicationUser>()
                          .WithMany()
                          .HasForeignKey("UserId")
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Broadcast>()
                          .WithMany()
                          .HasForeignKey("BroadcastId")
                          .OnDelete(DeleteBehavior.Cascade)
                );
        }



    }
}
