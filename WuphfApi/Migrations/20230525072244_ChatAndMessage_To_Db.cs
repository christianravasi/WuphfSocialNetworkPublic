using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class ChatAndMessage_To_Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUser1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FkUser2 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_FkUser1",
                        column: x => x.FkUser1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_FkUser2",
                        column: x => x.FkUser2,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Messaggi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdChat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messaggi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messaggi_AspNetUsers_Receiver",
                        column: x => x.Receiver,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Messaggi_AspNetUsers_Sender",
                        column: x => x.Sender,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Messaggi_Chats_IdChat",
                        column: x => x.IdChat,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_FkUser1",
                table: "Chats",
                column: "FkUser1");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_FkUser2",
                table: "Chats",
                column: "FkUser2");

            migrationBuilder.CreateIndex(
                name: "IX_Messaggi_IdChat",
                table: "Messaggi",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_Messaggi_Receiver",
                table: "Messaggi",
                column: "Receiver");

            migrationBuilder.CreateIndex(
                name: "IX_Messaggi_Sender",
                table: "Messaggi",
                column: "Sender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messaggi");

            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
