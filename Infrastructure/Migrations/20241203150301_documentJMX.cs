using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class documentJMX : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_JMX_Jmx_Id",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "Jmx_Id",
                table: "Documents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_JMX_Jmx_Id",
                table: "Documents",
                column: "Jmx_Id",
                principalTable: "JMX",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_JMX_Jmx_Id",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "Jmx_Id",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_JMX_Jmx_Id",
                table: "Documents",
                column: "Jmx_Id",
                principalTable: "JMX",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
