using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etiqa_Assessment_REST_API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phonenumber = table.Column<int>(type: "int", nullable: true),
                    skillsets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hobby = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.username);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
