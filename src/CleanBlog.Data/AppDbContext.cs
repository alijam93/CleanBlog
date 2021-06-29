using CleanBlog.Domain.Entities;
using CleanBlog.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid,
              IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
              IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
              : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
        }
    }
}
