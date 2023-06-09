using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class Likes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkPost = table.Column<int>(type: "int", nullable: true),
                    FkCommento = table.Column<int>(type: "int", nullable: true),
                    FkUser = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_FkUser",
                        column: x => x.FkUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Commenti_FkCommento",
                        column: x => x.FkCommento,
                        principalTable: "Commenti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Posts_FkPost",
                        column: x => x.FkPost,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_FkCommento",
                table: "Likes",
                column: "FkCommento");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_FkPost",
                table: "Likes",
                column: "FkPost");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_FkUser",
                table: "Likes",
                column: "FkUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
