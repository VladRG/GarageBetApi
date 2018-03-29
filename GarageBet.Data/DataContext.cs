using GarageBet.Domain.MM;
using GarageBet.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GarageBet.Domain
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChampionshipTeam>()
                .HasKey(entity => new { entity.ChampionshipId, entity.TeamId });

            builder.Entity<ChampionshipMatch>()
              .HasKey(entity => new { entity.ChampionshipId, entity.MatchId });
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Championship> Championships { get; set; }
        public DbSet<Team> Teams { get; set; }

    }
}
