using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class MakeHashedPasswordNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "cc24b6e6-12dd-4b72-93f9-2051031be8c6");

            migrationBuilder.AlterColumn<string>(
                name: "HashedPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "HashedPassword", "Role", "Username" },
                values: new object[] { "fc989b46-c6f3-424b-9791-3ce43cff0a90", "admin@d3akhtar.com", "$2a$11$Rbe7xECSirbfYcvLtBffVeVWe0WwdZoO.i4fdnuB3qRBlt9/iO48u", "admin", "d3akhtar-admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "fc989b46-c6f3-424b-9791-3ce43cff0a90");

            migrationBuilder.AlterColumn<string>(
                name: "HashedPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "HashedPassword", "Role", "Username" },
                values: new object[] { "cc24b6e6-12dd-4b72-93f9-2051031be8c6", "admin@d3akhtar.com", "$2a$11$QCzw0STFFJpGndxz6uvX4u8EqGo8JKZtFJwC9vM8lxyYjp1sX5fIa", "admin", "d3akhtar-admin" });
        }
    }
}
