using OptiPlanBackend.Models;
using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Enums;

namespace OptiPlanBackend.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        // Existing DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<TeamMembership> TeamMemberships { get; set; }
        public DbSet<Invitation> Invitations { get; set; }



        public DbSet<WorkItem> WorkItems { get; set; }


        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<WorkItemHistory> TaskHistories { get; set; }
        
        public DbSet<Sprint> Sprints { get; set; }


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
            modelBuilder.Entity<Models.WorkItem>().ToTable("WorkItems");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Attachment>().ToTable("Attachments");
            modelBuilder.Entity<WorkItemHistory>().ToTable("WorkItemHistories");

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

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

            modelBuilder.Entity<OptiPlanBackend.Models.WorkItem>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<OptiPlanBackend.Models.WorkItem>()
                .Property(t => t.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<OptiPlanBackend.Models.WorkItem>()
                .Property(t => t.Type)
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
                .WithMany(t => t.Invitations)
                .HasForeignKey(i => i.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- NEW TASK-RELATED RELATIONSHIPS ---

            // Project -> Tasks (One-to-Many)
            modelBuilder.Entity<OptiPlanBackend.Models.WorkItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)  // Add ICollection<Task> Tasks to Project model
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task -> Assigned User (Many-to-One)
            modelBuilder.Entity<Models.WorkItem>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)  // Add ICollection<Task> AssignedTasks to User model
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);  // Keep tasks if user is deleted

            // Task -> Reporter (Many-to-One)
            modelBuilder.Entity<Models.WorkItem>()
                .HasOne(t => t.Reporter)
                .WithMany(u => u.ReportedTasks)  // Add ICollection<Task> ReportedTasks to User model
                .HasForeignKey(t => t.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Task -> Comments (One-to-Many)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.WorkItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.WorkItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment -> Author (Many-to-One)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Task -> Attachments (One-to-Many)
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.WorkItem)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.WorkItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Attachment -> Uploader (Many-to-One)
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Uploader)
                .WithMany()
                .HasForeignKey(a => a.UploaderId)
                .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItemHistory>()
       .HasOne(h => h.WorkItem)
       .WithMany(t => t.History)
       .HasForeignKey(h => h.WorkItemId);


            // History -> ChangedBy (Many-to-One)
            modelBuilder.Entity<WorkItemHistory>()
                .HasOne(th => th.ChangedBy)
                .WithMany()
                .HasForeignKey(th => th.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);


            // Add these configurations for Invitation relationships
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Inviter)
                .WithMany(u => u.SentInvitations)
                .HasForeignKey(i => i.InviterId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Invitee)
                .WithMany(u => u.ReceivedInvitations)
                .HasForeignKey(i => i.InviteeId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete
        }
    }
}