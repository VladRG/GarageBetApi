﻿// <auto-generated />
using GarageBet.Data;
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
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Database.MM.ChampionshipTeam", b =>
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

            modelBuilder.Entity("GarageBet.Domain.Tables.Bet", b =>
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

            modelBuilder.Entity("GarageBet.Domain.Tables.Championship", b =>
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

            modelBuilder.Entity("GarageBet.Domain.Tables.Match", b =>
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

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("ChampionshipId");

                    b.HasIndex("HomeTeamId", "AwayTeamId", "ChampionshipId")
                        .IsUnique();

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("GarageBet.Domain.Tables.Team", b =>
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

            modelBuilder.Entity("GarageBet.Domain.Tables.User", b =>
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

            modelBuilder.Entity("GarageBet.Domain.Tables.UserClaim", b =>
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

            modelBuilder.Entity("Database.MM.ChampionshipTeam", b =>
                {
                    b.HasOne("GarageBet.Domain.Tables.Championship", "Championship")
                        .WithMany("ChampionshipTeams")
                        .HasForeignKey("ChampionshipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GarageBet.Domain.Tables.Team", "Team")
                        .WithMany("ChampionshipTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GarageBet.Domain.Tables.Bet", b =>
                {
                    b.HasOne("GarageBet.Domain.Tables.Match", "Match")
                        .WithMany("Bets")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GarageBet.Domain.Tables.User", "User")
                        .WithMany("Bets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GarageBet.Domain.Tables.Match", b =>
                {
                    b.HasOne("GarageBet.Domain.Tables.Team", "AwayTeamNavigationProperty")
                        .WithMany("AwayMatches")
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("GarageBet.Domain.Tables.Championship", "ChampionshipNavigationProperty")
                        .WithMany("Matches")
                        .HasForeignKey("ChampionshipId");

                    b.HasOne("GarageBet.Domain.Tables.Team", "HomeTeamNavigationProperty")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomeTeamId");
                });

            modelBuilder.Entity("GarageBet.Domain.Tables.UserClaim", b =>
                {
                    b.HasOne("GarageBet.Domain.Tables.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
