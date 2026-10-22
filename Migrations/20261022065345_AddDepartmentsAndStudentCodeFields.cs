using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentsAndStudentCodeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdmissionYear",
                table: "Student",
                type: "INTEGER",
                nullable: false,
                defaultValue: 2026);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: "CNTT");

            migrationBuilder.AddColumn<string>(
                name: "MajorCode",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: "05");

            migrationBuilder.AddColumn<string>(
                name: "TrainingSystemCode",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: "21");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentCode);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentCode", "Name" },
                values: new object[,]
                {
                    { "CD", "Khoa Cơ - Điện" },
                    { "CNTT", "Khoa Công nghệ thông tin" },
                    { "DC", "Khoa Địa chất" },
                    { "DK", "Khoa Dầu khí" },
                    { "GDQP", "Khoa Giáo dục quốc phòng" },
                    { "KHCB", "Khoa Khoa học cơ bản" },
                    { "KT", "Khoa Kinh tế và Quản trị kinh doanh" },
                    { "LLCT", "Khoa Lý luận chính trị" },
                    { "M", "Khoa Mỏ" },
                    { "MT", "Khoa Môi trường" },
                    { "TDB", "Khoa Trắc địa - Bản đồ và Quản lý đất đai" },
                    { "XD", "Khoa Xây dựng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_DepartmentCode",
                table: "Student",
                column: "DepartmentCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Departments_DepartmentCode",
                table: "Student",
                column: "DepartmentCode",
                principalTable: "Departments",
                principalColumn: "DepartmentCode",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Departments_DepartmentCode",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Student_DepartmentCode",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "AdmissionYear",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "MajorCode",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "TrainingSystemCode",
                table: "Student");
        }
    }
}
