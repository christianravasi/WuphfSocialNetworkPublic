using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedUserAndPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Immagine",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoProfilo",
                table: "LocalUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoProfilo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Immagine",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FotoProfilo",
                table: "LocalUsers");

            migrationBuilder.DropColumn(
                name: "FotoProfilo",
                table: "AspNetUsers");
        }
    }
}
