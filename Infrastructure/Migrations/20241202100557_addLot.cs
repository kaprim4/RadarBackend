using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentSummary_DeploymentData_DeploymentData_Id",
                table: "DeploymentSummary");

            migrationBuilder.DropForeignKey(
                name: "FK_Device_Device_DeviceId",
                table: "Device");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleData_DeploymentData_DeploymentData_Id",
                table: "VehicleData");

            migrationBuilder.DropTable(
                name: "DeploymentData");

            migrationBuilder.DropTable(
                name: "Treatment");

            migrationBuilder.DropIndex(
                name: "IX_Device_DeviceId",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Device");

            migrationBuilder.RenameColumn(
                name: "DeploymentData_Id",
                table: "VehicleData",
                newName: "JMX_Id");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleData_DeploymentData_Id",
                table: "VehicleData",
                newName: "IX_VehicleData_JMX_Id");

            migrationBuilder.RenameColumn(
                name: "DeploymentData_Id",
                table: "DeploymentSummary",
                newName: "JMX_Id");

            migrationBuilder.RenameIndex(
                name: "IX_DeploymentSummary_DeploymentData_Id",
                table: "DeploymentSummary",
                newName: "IX_DeploymentSummary_JMX_Id");

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lots_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    Jmx_Id = table.Column<int>(type: "integer", nullable: false),
                    Lot_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Lots_Lot_Id",
                        column: x => x.Lot_Id,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JMX",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Document_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JMX", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JMX_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JMX_Documents_Document_Id",
                        column: x => x.Document_Id,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Jmx_Id",
                table: "Documents",
                column: "Jmx_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Lot_Id",
                table: "Documents",
                column: "Lot_Id");

            migrationBuilder.CreateIndex(
                name: "IX_JMX_Document_Id",
                table: "JMX",
                column: "Document_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_DeviceId",
                table: "Lots",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentSummary_JMX_JMX_Id",
                table: "DeploymentSummary",
                column: "JMX_Id",
                principalTable: "JMX",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleData_JMX_JMX_Id",
                table: "VehicleData",
                column: "JMX_Id",
                principalTable: "JMX",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_JMX_Jmx_Id",
                table: "Documents",
                column: "Jmx_Id",
                principalTable: "JMX",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentSummary_JMX_JMX_Id",
                table: "DeploymentSummary");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleData_JMX_JMX_Id",
                table: "VehicleData");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_JMX_Jmx_Id",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "JMX");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.RenameColumn(
                name: "JMX_Id",
                table: "VehicleData",
                newName: "DeploymentData_Id");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleData_JMX_Id",
                table: "VehicleData",
                newName: "IX_VehicleData_DeploymentData_Id");

            migrationBuilder.RenameColumn(
                name: "JMX_Id",
                table: "DeploymentSummary",
                newName: "DeploymentData_Id");

            migrationBuilder.RenameIndex(
                name: "IX_DeploymentSummary_JMX_Id",
                table: "DeploymentSummary",
                newName: "IX_DeploymentSummary_DeploymentData_Id");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Device",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Treatment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatment_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeploymentData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Treatment_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeploymentData_BaseEntity_Id",
                        column: x => x.Id,
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeploymentData_Treatment_Treatment_Id",
                        column: x => x.Treatment_Id,
                        principalTable: "Treatment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceId",
                table: "Device",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentData_Treatment_Id",
                table: "DeploymentData",
                column: "Treatment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentSummary_DeploymentData_DeploymentData_Id",
                table: "DeploymentSummary",
                column: "DeploymentData_Id",
                principalTable: "DeploymentData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Device_Device_DeviceId",
                table: "Device",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleData_DeploymentData_DeploymentData_Id",
                table: "VehicleData",
                column: "DeploymentData_Id",
                principalTable: "DeploymentData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
