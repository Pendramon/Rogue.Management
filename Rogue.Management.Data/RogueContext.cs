using Microsoft.EntityFrameworkCore;
using Rogue.Management.Data.Model;

namespace Rogue.Management.Data
{
    public class RogueContext : DbContext
    {
        public RogueContext(DbContextOptions<RogueContext> options)
            : base(options)
        {
        }

        public DbSet<UserDto> Users => this.Set<UserDto>();

        public DbSet<ProjectDto> Projects => this.Set<ProjectDto>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserDto>().ToTable("Users");
            builder.Entity<UserDto>().HasKey(u => u.Id);
            builder.Entity<UserDto>().Property(u => u.Id).HasColumnName("UserId").ValueGeneratedOnAdd();
            builder.Entity<UserDto>().Property(u => u.Username).IsRequired().HasMaxLength(30);
            builder.Entity<UserDto>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<UserDto>().Property(u => u.Email).IsRequired().HasMaxLength(254);
            builder.Entity<UserDto>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<UserDto>().Property(u => u.PasswordHash).IsRequired();
            builder.Entity<UserDto>().Property(u => u.CreatedAt).IsRequired();

            builder.Entity<ProjectDto>().ToTable("Projects");
            builder.Entity<ProjectDto>().HasKey(p => p.Id);
            builder.Entity<ProjectDto>().Property(p => p.Id).HasColumnName("ProjectId").ValueGeneratedOnAdd();
            builder.Entity<ProjectDto>().HasOne(p => p.Owner).WithMany(u => u.Projects);
            builder.Entity<ProjectDto>().Property(p => p.Name).IsRequired();
            builder.Entity<ProjectDto>().Property(p => p.CreatedAt).IsRequired();
        }
    }
}