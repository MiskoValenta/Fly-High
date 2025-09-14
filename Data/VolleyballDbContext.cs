using FlyHigh.Models;
using FlyHigh.Models.MatchModels;
using FlyHigh.Models.TeamModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DbMatch = FlyHigh.Models.MatchModels.Match;

namespace FlyHigh.Data
{
    public class VolleyballDbContext : IdentityDbContext<User>
    {
        public VolleyballDbContext(DbContextOptions<VolleyballDbContext> options)
            : base(options)
        {
        }

        // Původní DbSets
        public DbSet<DbMatch> Matches { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<MatchResult> MatchResults { get; set; }
        public DbSet<SetPoint> SetPoints { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingAttendance> TrainingAttendances { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<PollResponse> PollResponses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TeamStatistics> TeamStatistics { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
        public DbSet<MatchAttendance> MatchAttendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Match - jen složité vztahy a indexy
            modelBuilder.Entity<DbMatch>(entity =>
            {
                // ✅ OPRAVA - Ignore problematické navigation properties
                // entity.Ignore(m => m.TeamAPlayers);
                // entity.Ignore(m => m.TeamBPlayers);

                // složité FK vztahy (nejdou v Data Annotations)
                entity.HasOne(m => m.HomeTeamEntity)
                      .WithMany(t => t.HomeMatches)
                      .HasForeignKey(m => m.HomeTeamId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.AwayTeamEntity)
                      .WithMany(t => t.AwayMatches)
                      .HasForeignKey(m => m.AwayTeamId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(m => m.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // indexy pro výkon
                entity.HasIndex(e => e.MatchDate);
                entity.HasIndex(e => e.HomeTeamId);
                entity.HasIndex(e => e.AwayTeamId);
                entity.HasIndex(e => e.MatchIdentifier);
            });

            // Set - jen vztahy a unique constraint
            modelBuilder.Entity<Set>(entity =>
            {
                entity.HasOne(s => s.Match)
                      .WithMany(m => m.Sets)
                      .HasForeignKey(s => s.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.MatchId, e.SetNumber }).IsUnique();
            });

            // Player - jen enum konverze
            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(p => p.Team).HasConversion<string>();
                entity.Property(p => p.Position).HasConversion<string>();
                entity.HasIndex(e => new { e.MatchId, e.Team, e.JerseyNumber });
            });

            // TeamMember - enum + složité indexy
            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.Property(e => e.Role).HasConversion<string>();

                entity.HasIndex(e => new { e.UserId, e.TeamId, e.IsActive });
                entity.HasIndex(e => new { e.TeamId, e.JerseyNumber }).IsUnique();

                entity.HasOne(tm => tm.User)
                      .WithMany(u => u.TeamMemberships)
                      .HasForeignKey(tm => tm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tm => tm.Team)
                      .WithMany(t => t.Members)
                      .HasForeignKey(tm => tm.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Team - jen indexy
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.City);
            });

            // Training - jen vztahy a indexy
            modelBuilder.Entity<Training>(entity =>
            {
                entity.HasOne(t => t.Team)
                      .WithMany(team => team.Trainings)
                      .HasForeignKey(t => t.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.TeamId, e.ScheduledDate });
            });

            // TrainingAttendance - enum + unique constraint
            modelBuilder.Entity<TrainingAttendance>(entity =>
            {
                entity.Property(e => e.Status).HasConversion<string>();

                entity.HasOne(ta => ta.Training)
                      .WithMany(t => t.Attendances)
                      .HasForeignKey(ta => ta.TrainingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ta => ta.User)
                      .WithMany()
                      .HasForeignKey(ta => ta.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.TrainingId, e.UserId }).IsUnique();
            });

            // Poll - enum + indexy
            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.Type).HasConversion<string>();

                entity.HasOne(p => p.Team)
                      .WithMany(t => t.Polls)
                      .HasForeignKey(p => p.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.CreatedByUser)
                      .WithMany()
                      .HasForeignKey(p => p.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.TeamId, e.CreatedAt });
                entity.HasIndex(e => e.ExpiresAt);
            });

            // PollOption - jen vztah a index
            modelBuilder.Entity<PollOption>(entity =>
            {
                entity.HasOne(po => po.Poll)
                      .WithMany(p => p.Options)
                      .HasForeignKey(po => po.PollId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.PollId, e.Order });
            });

            // PollResponse - jen vztahy a index
            modelBuilder.Entity<PollResponse>(entity =>
            {
                entity.HasOne(pr => pr.Poll)
                      .WithMany(p => p.Responses)
                      .HasForeignKey(pr => pr.PollId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pr => pr.PollOption)
                      .WithMany(po => po.Responses)
                      .HasForeignKey(pr => pr.PollOptionId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pr => pr.User)
                      .WithMany(u => u.PollResponses)
                      .HasForeignKey(pr => pr.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.PollId, e.UserId, e.PollOptionId });
            });

            // Notification - jen enum a indexy
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Type).HasConversion<string>();

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.IsRead });
                entity.HasIndex(e => e.CreatedAt);
            });

            // TeamStatistics - jen unique constraint
            modelBuilder.Entity<TeamStatistics>(entity =>
            {
                entity.HasOne(ts => ts.Team)
                      .WithMany()
                      .HasForeignKey(ts => ts.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.TeamId, e.Season }).IsUnique();
            });

            // PlayerStatistics - jen unique constraint  
            modelBuilder.Entity<PlayerStatistics>(entity =>
            {
                entity.HasOne(ps => ps.User)
                      .WithMany()
                      .HasForeignKey(ps => ps.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ps => ps.Team)
                      .WithMany()
                      .HasForeignKey(ps => ps.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.TeamId, e.Season }).IsUnique();
            });

            // MatchAttendance - jen enum a unique constraint
            modelBuilder.Entity<MatchAttendance>(entity =>
            {
                entity.Property(e => e.Status).HasConversion<string>();

                entity.HasOne(ma => ma.Match)
                      .WithMany(m => m.Attendances)
                      .HasForeignKey(ma => ma.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ma => ma.User)
                      .WithMany()
                      .HasForeignKey(ma => ma.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.MatchId, e.UserId }).IsUnique();
            });
        }
    }
}
