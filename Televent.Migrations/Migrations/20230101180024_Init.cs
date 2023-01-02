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
                name: "users",
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
                    building = table.Column<int>(type: "integer", nullable: true),
                    room = table.Column<int>(type: "integer", nullable: true),
                    additionalinfo = table.Column<string>(name: "additional_info", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_users_ward_id",
                        column: x => x.wardid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_ward_id",
                table: "users",
                column: "ward_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
