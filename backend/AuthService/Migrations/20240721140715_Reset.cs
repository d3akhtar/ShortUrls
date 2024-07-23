using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class Reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "HashedPassword", "PasswordResetToken", "ResetTokenExpires", "Role", "Username", "VerificationToken", "VerifiedAt" },
                values: new object[] { 1, new DateTime(2024, 7, 21, 10, 7, 14, 837, DateTimeKind.Local).AddTicks(9449), "admin@d3akhtar.com", "$2a$11$lzvc3qDe3cT3P8XJUSaXnOc.KhXM/EX84kht63dqdeRj/6SZrkyxm", null, null, "admin", "d3akhtar-admin", "E5F2898FA521D76F8F89A4CAF89D3702D402FDE346DE220C486957C08C3573D434848A49C805CC4830E3BEF16B8D1298C565BE8AA7AF3756AA7C3B68710945F3", new DateTime(2024, 7, 21, 10, 7, 14, 837, DateTimeKind.Local).AddTicks(9499) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
