using OptiPlanBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace OptiPlanBackend.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMembership> TeamMemberships { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<TeamMembership>().ToTable("TeamMemberships");
            modelBuilder.Entity<UserProfile>().ToTable("UserProfiles");
            modelBuilder.Entity<Invitation>().ToTable("Invitations");

            // Configure enums to be stored as strings
            modelBuilder.Entity<TeamMembership>()
                .Property(tm => tm.Status)
                .HasConversion<string>();

            modelBuilder.Entity<TeamMembership>()
                .Property(tm => tm.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Configure relationships with proper delete behaviors
            // User -> Projects (Owner)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.OwnedProjects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project -> Team (One-to-One)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Team)
                .WithOne(t => t.Project)
                .HasForeignKey<Team>(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Team -> Members (One-to-Many)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Members)
                .WithOne(tm => tm.Team)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> TeamMemberships
            modelBuilder.Entity<TeamMembership>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Profile (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Team -> Invitations
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Team)
                .WithMany()
                .HasForeignKey(i => i.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}