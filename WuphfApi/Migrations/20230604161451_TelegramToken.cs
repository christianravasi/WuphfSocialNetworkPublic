using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class TelegramToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelegramToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramToken",
                table: "AspNetUsers");
        }
    }
}
