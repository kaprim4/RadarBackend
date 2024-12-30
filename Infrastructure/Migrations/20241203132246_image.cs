using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleData_Images_ImagesId",
                table: "VehicleData");

            migrationBuilder.DropIndex(
                name: "IX_VehicleData_ImagesId",
                table: "VehicleData");

            migrationBuilder.DropColumn(
                name: "ImagesId",
                table: "VehicleData");

            migrationBuilder.RenameColumn(
                name: "Image_Name",
                table: "VehicleData",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "VehicleData",
                newName: "Image_Name");

            migrationBuilder.AddColumn<int>(
                name: "ImagesId",
                table: "VehicleData",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleData_ImagesId",
                table: "VehicleData",
                column: "ImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleData_Images_ImagesId",
                table: "VehicleData",
                column: "ImagesId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
