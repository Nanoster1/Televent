using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Televent.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    eventname = table.Column<string>(name: "event_name", type: "text", nullable: false),
                    eventdescription = table.Column<string>(name: "event_description", type: "text", nullable: false),
                    executiontime = table.Column<DateTimeOffset>(name: "execution_time", type: "timestamp with time zone", nullable: true),
                    isexecuted = table.Column<bool>(name: "is_executed", type: "boolean", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    isregistered = table.Column<bool>(name: "is_registered", type: "boolean", nullable: false),
                    wardid = table.Column<long>(name: "ward_id", type: "bigint", nullable: true),
                    nameandsurname = table.Column<string>(name: "name_and_surname", type: "text", nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true),
                    squad = table.Column<int>(type: "integer", nullable: true),
                    building = table.Column<string>(type: "varchar", nullable: true),
                    room = table.Column<int>(type: "integer", nullable: true),
                    additionalinfo = table.Column<string>(name: "additional_info", type: "varchar", nullable: true),
                    chatid = table.Column<long>(name: "chat_id", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
