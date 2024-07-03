using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortUrlService.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAlias = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "ShortUrls",
                columns: new[] { "Code", "DestinationUrl", "IsAlias" },
                values: new object[] { "test", "https://www.youtube.com/watch?v=UyPnhOpngRA&ab_channel=Toast", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrls");
        }
    }
}
