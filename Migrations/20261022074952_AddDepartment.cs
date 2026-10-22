using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode1",
                table: "Departments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "CD",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "CNTT",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "DC",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "DK",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "GDQP",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "KHCB",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "KT",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "LLCT",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "M",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "MT",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "TDB",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentCode",
                keyValue: "XD",
                column: "DepartmentCode1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentCode1",
                table: "Departments",
                column: "DepartmentCode1");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Departments_DepartmentCode1",
                table: "Departments",
                column: "DepartmentCode1",
                principalTable: "Departments",
                principalColumn: "DepartmentCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Departments_DepartmentCode1",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentCode1",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentCode1",
                table: "Departments");
        }
    }
}
