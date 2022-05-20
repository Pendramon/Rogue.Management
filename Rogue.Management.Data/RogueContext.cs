using Microsoft.EntityFrameworkCore;
using Rogue.Data.Model;

namespace Rogue.Management.Data
{
    public class RogueContext : DbContext
    {
        public RogueContext(DbContextOptions<RogueContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => this.Set<User>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().Property(u => u.Id).HasColumnName("UserId").ValueGeneratedOnAdd();
            builder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(30);
            builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(254);
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            builder.Entity<User>().Property(u => u.CreatedAt).IsRequired();
        }
    }
}