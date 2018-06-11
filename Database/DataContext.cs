using GarageBet.Api.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GarageBet.Api.Database
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DataContext(DbContextOptionsBuilder<DataContext> options)
            : base(options.Options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureIndexes(builder);
            ConfigureChampionshipTeam(builder);
            ConfigureLeaderboardUser(builder);
            ConfigureMatches(builder);
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseMySql("Server=localhost;database=garagebet;user=root;pwd=", config =>
            //{
            //   config.MigrationsAssembly("GarageBet.Api");
            //});
            base.OnConfiguring(options);
        }


        #region Tables
        public DbSet<User> Users { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }

        public DbSet<Championship> Championships { get; set; }

        public DbSet<ChampionshipTeam> ChampionshipTeams { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Leaderboard> Leaderboards { get; set; }

        public DbSet<LeaderboardUser> LeaderboardUsers { get; set; }
        #endregion

        #region Configuration
        private void ConfigureMatches(ModelBuilder builder)
        {
            builder.Entity<Championship>()
               .HasMany(row => row.Matches)
               .WithOne(championship => championship.Championship)
               .HasForeignKey("ChampionshipId");

            builder.Entity<Team>()
                .HasMany(row => row.HomeMatches)
                .WithOne(team => team.HomeTeam)
                .HasForeignKey("HomeTeamId");

            builder.Entity<Team>()
                .HasMany(row => row.AwayMatches)
                .WithOne(team => team.AwayTeam)
                .HasForeignKey("AwayTeamId");
        }

        private void ConfigureChampionshipTeam(ModelBuilder builder)
        {
            builder.Entity<ChampionshipTeam>()
                .HasKey(row => new { row.ChampionshipId, row.TeamId });

            builder.Entity<ChampionshipTeam>()
                   .HasOne(e => e.Team)
                   .WithMany(row => row.ChampionshipTeams);


            builder.Entity<ChampionshipTeam>()
                .HasOne(e => e.Championship)
                .WithMany(row => row.ChampionshipTeams);
        }

        private void ConfigureLeaderboardUser(ModelBuilder builder)
        {
            builder.Entity<LeaderboardUser>()
                .HasKey(row => new { row.LeaderboardId, row.UserId });

            builder.Entity<LeaderboardUser>()
                .HasOne(e => e.Leaderboard)
                .WithMany(e => e.Users);

            builder.Entity<LeaderboardUser>()
                .HasOne(e => e.User)
                .WithMany(e => e.Leaderboards);

            builder.Entity<Leaderboard>()
                .HasOne(row => row.Admin)
                .WithMany(row => row.ManagedLeaderboards);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            builder.Entity<Bet>()
                .HasIndex(row => new { row.UserId, row.MatchId })
                .IsUnique();

            builder.Entity<Championship>()
                .HasIndex(row => new { row.Name, row.CompetitiveYear })
                .IsUnique();

            builder.Entity<Team>()
                .HasIndex(row => new { row.Name })
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(entity => entity.Email)
                .IsUnique();

            builder.Entity<UserClaim>()
                .HasIndex(row => new { row.ClaimType, row.UserId })
                .IsUnique();
        }
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

