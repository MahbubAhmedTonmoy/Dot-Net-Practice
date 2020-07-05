using FBLite.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBLite.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        //Add-Migration first Update-Database
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(p => p.Posts)
                .WithOne(u => u.User);
            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(u => u.Post);
            builder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(u => u.Post);
        }
    }
}
