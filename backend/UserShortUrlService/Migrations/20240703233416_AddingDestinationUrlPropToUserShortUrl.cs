using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserShortUrlService.Migrations
{
    /// <inheritdoc />
    public partial class AddingDestinationUrlPropToUserShortUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationUrl",
                table: "UserShortUrls",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationUrl",
                table: "UserShortUrls");
        }
    }
}
