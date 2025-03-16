using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce511.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderShipedStatusToOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OrderShipedStatus",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderShipedStatus",
                table: "Orders");
        }
    }
}
