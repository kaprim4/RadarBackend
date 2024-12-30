using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeDeviceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentSummary_Device_DeviceId",
                table: "DeploymentSummary");

            migrationBuilder.DropIndex(
                name: "IX_DeploymentSummary_DeviceId",
                table: "DeploymentSummary");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "DeploymentSummary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "DeploymentSummary",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentSummary_DeviceId",
                table: "DeploymentSummary",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentSummary_Device_DeviceId",
                table: "DeploymentSummary",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
