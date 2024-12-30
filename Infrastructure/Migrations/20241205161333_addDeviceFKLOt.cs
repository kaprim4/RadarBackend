using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDeviceFKLOt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Device_DeviceId",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_DeviceId",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Lots");

            migrationBuilder.AddColumn<int>(
                name: "Device_Id",
                table: "Lots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "JMX",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_Device_Id",
                table: "Lots",
                column: "Device_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Device_Device_Id",
                table: "Lots",
                column: "Device_Id",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Device_Device_Id",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_Device_Id",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "Device_Id",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "JMX");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Lots",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_DeviceId",
                table: "Lots",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Device_DeviceId",
                table: "Lots",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");
        }
    }
}
