﻿// <auto-generated />
using GarageBet.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace GarageBet.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180611095445_Leaderboards")]
    partial class Leaderboards
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("GarageBet.Api.Database.ChampionshipTeam", b =>
                {
                    b.Property<long>("ChampionshipId");

                    b.Property<long>("TeamId");

                    b.Property<int>("Draw");

                    b.Property<int>("Lost");

                    b.Property<int>("Win");

                    b.HasKey("ChampionshipId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("ChampionshipTeams");
                });

            modelBuilder.Entity("GarageBet.Api.Database.LeaderboardUser", b =>
                {
                    b.Property<long>("LeaderboardId");

                    b.Property<long>("UserId");

                    b.Property<bool?>("Accepted");

                    b.HasKey("LeaderboardId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LeaderboardUsers");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Bet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("AwayScore");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<short>("HomeScore");

                    b.Property<long>("MatchId");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("UserId", "MatchId")
                        .IsUnique();

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Championship", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompetitiveYear")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("Name", "CompetitiveYear")
                        .IsUnique();

                    b.ToTable("Championships");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Leaderboard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AdminId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Leaderboards");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Match", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("AwayScore");

                    b.Property<long?>("AwayTeamId");

                    b.Property<long?>("ChampionshipId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("DateTime");

                    b.Property<short>("HomeScore");

                    b.Property<long?>("HomeTeamId");

                    b.Property<string>("Standing");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("ChampionshipId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Team", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<DateTime>("LastLogin");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Token");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.UserClaim", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ClaimType", "UserId")
                        .IsUnique();

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("GarageBet.Api.Database.ChampionshipTeam", b =>
                {
                    b.HasOne("GarageBet.Api.Database.Tables.Championship", "Championship")
                        .WithMany("ChampionshipTeams")
                        .HasForeignKey("ChampionshipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GarageBet.Api.Database.Tables.Team", "Team")
                        .WithMany("ChampionshipTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GarageBet.Api.Database.LeaderboardUser", b =>
                {
                    b.HasOne("GarageBet.Api.Database.Tables.Leaderboard", "Leaderboard")
                        .WithMany("Users")
                        .HasForeignKey("LeaderboardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GarageBet.Api.Database.Tables.User", "User")
                        .WithMany("Leaderboards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Bet", b =>
                {
                    b.HasOne("GarageBet.Api.Database.Tables.Match", "Match")
                        .WithMany("Bets")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GarageBet.Api.Database.Tables.User", "User")
                        .WithMany("Bets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Leaderboard", b =>
                {
                    b.HasOne("GarageBet.Api.Database.Tables.User", "Admin")
                        .WithMany("ManagedLeaderboards")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.Match", b =>
                {
                    b.HasOne("GarageBet.Api.Database.Tables.Team", "AwayTeam")
                        .WithMany("AwayMatches")
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("GarageBet.Api.Database.Tables.Championship", "Championship")
                        .WithMany("Matches")
                        .HasForeignKey("ChampionshipId");

                    b.HasOne("GarageBet.Api.Database.Tables.Team", "HomeTeam")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomeTeamId");
                });

            modelBuilder.Entity("GarageBet.Api.Database.Tables.UserClaim", b =>
                {
                    b.HasOne("GarageBet.Api.Database.Tables.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}