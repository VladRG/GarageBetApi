using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GarageBet.Api.Migrations
{
    public partial class Leaderboards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Championships",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompetitiveYear = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Championships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChampionshipTeams",
                columns: table => new
                {
                    ChampionshipId = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    Draw = table.Column<int>(nullable: false),
                    Lost = table.Column<int>(nullable: false),
                    Win = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionshipTeams", x => new { x.ChampionshipId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_ChampionshipTeams_Championships_ChampionshipId",
                        column: x => x.ChampionshipId,
                        principalTable: "Championships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChampionshipTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AwayScore = table.Column<short>(nullable: false),
                    AwayTeamId = table.Column<long>(nullable: true),
                    ChampionshipId = table.Column<long>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    HomeScore = table.Column<short>(nullable: false),
                    HomeTeamId = table.Column<long>(nullable: true),
                    Standing = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Championships_ChampionshipId",
                        column: x => x.ChampionshipId,
                        principalTable: "Championships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leaderboards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaderboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaderboards_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AwayScore = table.Column<short>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    HomeScore = table.Column<short>(nullable: false),
                    MatchId = table.Column<long>(nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardUsers",
                columns: table => new
                {
                    LeaderboardId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Accepted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardUsers", x => new { x.LeaderboardId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LeaderboardUsers_Leaderboards_LeaderboardId",
                        column: x => x.LeaderboardId,
                        principalTable: "Leaderboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderboardUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_MatchId",
                table: "Bets",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserId_MatchId",
                table: "Bets",
                columns: new[] { "UserId", "MatchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Championships_Name_CompetitiveYear",
                table: "Championships",
                columns: new[] { "Name", "CompetitiveYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChampionshipTeams_TeamId",
                table: "ChampionshipTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboards_AdminId",
                table: "Leaderboards",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardUsers_UserId",
                table: "LeaderboardUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ChampionshipId",
                table: "Matches",
                column: "ChampionshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_ClaimType_UserId",
                table: "UserClaims",
                columns: new[] { "ClaimType", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "ChampionshipTeams");

            migrationBuilder.DropTable(
                name: "LeaderboardUsers");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Leaderboards");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Championships");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
