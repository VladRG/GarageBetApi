using Database.MM;
using Database.Views;
using GarageBet.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GarageBet.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DataContext(DbContextOptionsBuilder<DataContext> options)
            : base(options.Options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // M:M Relationships
            builder.Entity<UserRole>()
                .HasKey(row => new { row.UserId, row.RoleId });

            builder.Entity<ChampionshipTeam>()
                .HasKey(row => new { row.ChampionshipId, row.TeamId });

            builder.Entity<ChampionshipTeam>()
                   .HasOne(e => e.Team)
                   .WithMany("ChampionshipTeams");

            builder.Entity<ChampionshipTeam>()
                .HasOne(e => e.Championship)
                .WithMany("ChampionshipTeams");

            // Entities
            builder.Entity<User>()
                .HasIndex(entity => entity.Email)
                .IsUnique();

            builder.Entity<Championship>()
                .HasIndex(row => new { row.Name, row.CompetitiveYear })
                .IsUnique();

            builder.Entity<Team>()
                .HasIndex(row => new { row.Name })
                .IsUnique();

            builder.Entity<Match>()
                .HasIndex(row => new { row.HomeTeamId, row.AwayTeamId, row.ChampionshipId })
                .IsUnique();

            builder.Entity<Bet>()
                .HasIndex(row => new { row.UserId, row.MatchId })
                .IsUnique();

            builder.Entity<Match>()
                .HasOne(row => row.HomeTeam)
                .WithMany(row => row.HomeMatches)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(row => row.AwayTeam)
                .WithMany(row => row.AwayMatches)
                .OnDelete(DeleteBehavior.Restrict);
            // Views
            builder.Query<MatchBet>()
                .ToTable("MatchBetsView");

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GarageBet;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                settings => settings.MigrationsAssembly("GarageBet.Api")
                );

            base.OnConfiguring(options);
        }


        #region Tables
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Championship> Championships { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Match> Matches { get; set; }
        #endregion

        #region Views
        public DbQuery<MatchBet> MatchBets { get; set; }
        #endregion

    }

    public class MigrationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            return new DataContext(builder);
        }
    }
}
