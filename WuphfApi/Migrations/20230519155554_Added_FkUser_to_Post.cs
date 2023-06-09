using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class Added_FkUser_to_Post : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FkUser",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FkUser",
                table: "Posts",
                column: "FkUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_FkUser",
                table: "Posts",
                column: "FkUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_FkUser",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_FkUser",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FkUser",
                table: "Posts");
        }
    }
}
