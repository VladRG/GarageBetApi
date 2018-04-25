﻿// <auto-generated />
using System;
using GarageBet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace GarageBet.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180424131220_initial2")]
    partial class initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-preview1-28290")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Database.MM.ChampionshipTeam", b =>
                {
                    b.Property<long>("ChampionshipId");

                    b.Property<long>("TeamId");

                    b.HasKey("ChampionshipId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("ChampionshipTeams");
                });

            modelBuilder.Entity("Database.MM.UserRole", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
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
                        .IsUnique()
                        .HasFilter("[HomeTeamId] IS NOT NULL AND [AwayTeamId] IS NOT NULL AND [ChampionshipId] IS NOT NULL");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("GarageBet.Domain.Tables.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Roles");
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

            modelBuilder.Entity("Database.MM.UserRole", b =>
                {
                    b.HasOne("GarageBet.Domain.Tables.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GarageBet.Domain.Tables.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
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
                    b.HasOne("GarageBet.Domain.Tables.Team", "AwayTeam")
                        .WithMany("AwayMatches")
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GarageBet.Domain.Tables.Championship", "Championship")
                        .WithMany("Matches")
                        .HasForeignKey("ChampionshipId");

                    b.HasOne("GarageBet.Domain.Tables.Team", "HomeTeam")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}