using Microsoft.EntityFrameworkCore.Migrations;

namespace AllMarkt.Migrations
{
    public partial class AddPricetoOrderItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");
        }
    }
}
