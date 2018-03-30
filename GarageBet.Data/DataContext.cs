﻿using Database.MM;
using Database.Views;
using GarageBet.Domain.MM;
using GarageBet.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

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
            builder.Entity<ChampionshipTeam>()
                .HasKey(entity => new { entity.ChampionshipId, entity.TeamId });

            builder.Entity<ChampionshipMatch>()
                .HasKey(entity => new { entity.ChampionshipId, entity.MatchId });

            builder.Entity<UserRole>()
                .HasKey(entity => new { entity.RoleId, entity.UserId });

            builder.Entity<User>()
                .HasIndex(entity => entity.Email)
                .IsUnique();

            // Views
            builder.Query<MatchBet>()
                .ToTable("MatchBetsView");

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
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
}
