using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Televent.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Games : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    starttime = table.Column<DateTimeOffset>(name: "start_time", type: "timestamp with time zone", nullable: false),
                    playerscount = table.Column<int>(name: "players_count", type: "integer", nullable: false),
                    isfinished = table.Column<bool>(name: "is_finished", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
