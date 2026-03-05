using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GES_IdeaSystem.Domain;
using GES_IdeaSystem.Domain.Entities;


namespace GES_IdeaSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Vote>()
                .HasIndex(x => new { x.IdeaId, x.UserId })
                .IsUnique();
        }

        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
