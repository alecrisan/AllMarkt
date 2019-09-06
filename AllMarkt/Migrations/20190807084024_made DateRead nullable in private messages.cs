using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllMarkt.Migrations
{
    public partial class madeDateReadnullableinprivatemessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRead",
                table: "PrivateMessages",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRead",
                table: "PrivateMessages",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
