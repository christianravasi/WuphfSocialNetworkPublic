using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommento_To_Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataUpdate",
                table: "Posts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Commenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Testo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FkUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FkPost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commenti_AspNetUsers_FkUser",
                        column: x => x.FkUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commenti_Posts_FkPost",
                        column: x => x.FkPost,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commenti_FkPost",
                table: "Commenti",
                column: "FkPost");

            migrationBuilder.CreateIndex(
                name: "IX_Commenti_FkUser",
                table: "Commenti",
                column: "FkUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commenti");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataUpdate",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
