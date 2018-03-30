using Database.MM;
using Database.Views;
using GarageBet.Domain.MM;
using GarageBet.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GarageBet.Data
{
    public class MigrationDbContext : DbContext
    {
        public MigrationDbContext(DbContextOptionsBuilder<MigrationDbContext> options)
            : base(options.Options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChampionshipTeam>()
                .HasKey(entity => new { entity.ChampionshipId, entity.TeamId });

            builder.Entity<ChampionshipMatch>()
                .HasKey(entity => new { entity.ChampionshipId, entity.MatchId });

            builder.Entity<UserRole>()
                .HasKey(entity => new { entity.UserId, entity.RoleId });

            builder.Query<MatchBet>().ToTable("MatchBetsView");

            builder.Entity<User>()
                .HasIndex(entity => entity.Email)
                .IsUnique();

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(
               "server=localhost;port=3306;database=gb;uid=root;password=",
               settings =>
               {
                   settings.MigrationsAssembly("GarageBet.Data");
               });
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

    public class MigrationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<MigrationDbContext>
    {
        public MigrationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MigrationDbContext>();
            return new MigrationDbContext(builder);
        }
    }
}
