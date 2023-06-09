using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WuphfApi.Migrations
{
    /// <inheritdoc />
    public partial class DataInvioInMessaggio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataInvio",
                table: "Messaggi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataInvio",
                table: "Messaggi");
        }
    }
}
