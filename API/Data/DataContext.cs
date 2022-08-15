using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder bulider)
        {
            base.OnModelCreating(bulider);

            bulider.Entity<AppUser>()
                    .HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            bulider.Entity<AppRole>()
                       .HasMany(ur => ur.UserRoles)
                       .WithOne(u => u.Role)
                       .HasForeignKey(ur => ur.RoleId)
                       .IsRequired();


            bulider.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            bulider.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);


            bulider.Entity<UserLike>()
            .HasOne(s => s.LikedUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);

            bulider.Entity<Message>()
         .HasOne(s => s.Recipient)
         .WithMany(l => l.MessagesReceived)
         .OnDelete(DeleteBehavior.Restrict);

            bulider.Entity<Message>()
           .HasOne(s => s.Sender)
           .WithMany(l => l.MessagesSent)
           .OnDelete(DeleteBehavior.Restrict);

        }

    }
}