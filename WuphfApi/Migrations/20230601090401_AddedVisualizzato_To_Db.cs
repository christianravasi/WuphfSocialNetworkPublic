using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedVisualizzato_To_Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visualizzato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkPost = table.Column<int>(type: "int", nullable: false),
                    FkUser = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visualizzato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visualizzato_AspNetUsers_FkUser",
                        column: x => x.FkUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visualizzato_Posts_FkPost",
                        column: x => x.FkPost,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visualizzato_FkPost",
                table: "Visualizzato",
                column: "FkPost");

            migrationBuilder.CreateIndex(
                name: "IX_Visualizzato_FkUser",
                table: "Visualizzato",
                column: "FkUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visualizzato");
        }
    }
}
