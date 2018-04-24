using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GarageBet.Api.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChampionshipTeam_Championships_ChampionshipId",
                table: "ChampionshipTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_ChampionshipTeam_Teams_TeamId",
                table: "ChampionshipTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChampionshipTeam",
                table: "ChampionshipTeam");

            migrationBuilder.RenameTable(
                name: "ChampionshipTeam",
                newName: "ChampionshipTeams");

            migrationBuilder.RenameIndex(
                name: "IX_ChampionshipTeam_TeamId",
                table: "ChampionshipTeams",
                newName: "IX_ChampionshipTeams_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChampionshipTeams",
                table: "ChampionshipTeams",
                columns: new[] { "ChampionshipId", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionshipTeams_Championships_ChampionshipId",
                table: "ChampionshipTeams",
                column: "ChampionshipId",
                principalTable: "Championships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionshipTeams_Teams_TeamId",
                table: "ChampionshipTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChampionshipTeams_Championships_ChampionshipId",
                table: "ChampionshipTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ChampionshipTeams_Teams_TeamId",
                table: "ChampionshipTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChampionshipTeams",
                table: "ChampionshipTeams");

            migrationBuilder.RenameTable(
                name: "ChampionshipTeams",
                newName: "ChampionshipTeam");

            migrationBuilder.RenameIndex(
                name: "IX_ChampionshipTeams_TeamId",
                table: "ChampionshipTeam",
                newName: "IX_ChampionshipTeam_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChampionshipTeam",
                table: "ChampionshipTeam",
                columns: new[] { "ChampionshipId", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionshipTeam_Championships_ChampionshipId",
                table: "ChampionshipTeam",
                column: "ChampionshipId",
                principalTable: "Championships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionshipTeam_Teams_TeamId",
                table: "ChampionshipTeam",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
