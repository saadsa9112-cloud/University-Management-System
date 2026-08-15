using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NedAcademy.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseModelId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Faculty_FacultyModelId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Faculty");

            migrationBuilder.DropIndex(
                name: "IX_Students_CourseModelId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FacultyModelId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Course",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CourseModelId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FacultyModelId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Students",
                newName: "DepartmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fees",
                table: "Students",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Students",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RollNumber",
                table: "Students",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Courses",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeadOfDept = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Credits", "DepartmentId", "Description", "Duration", "Name" },
                values: new object[] { "BSCS", 4, 1, "4-year undergraduate degree in Computer Science", 48, "Bachelor of Science in Computer Science" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Code", "Credits", "DepartmentId", "Description", "Duration", "Name" },
                values: new object[] { "DIN", 3, 1, "Professional diploma in Computer Networking & Security", 12, "Diploma in Networking" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "HeadOfDept", "Name" },
                values: new object[,]
                {
                    { 1, "CS", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Department of Computer Science & Information Technology", "Dr. Ahmed Khan", "Computer Science" },
                    { 2, "EE", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Department of Electrical Engineering", "Dr. Sara Ali", "Electrical Engineering" },
                    { 3, "BBA", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Department of Business Administration & Management", "Prof. Zubair Sheikh", "Business Administration" }
                });

            migrationBuilder.InsertData(
                table: "Notices",
                columns: new[] { "Id", "Content", "CreatedAt", "IsActive", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "We warmly welcome all new and returning students to NED Academy for the academic session 2024-2025. Please report to your respective departments by September 5th.", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Welcome to NED Academy — Session 2024-25" },
                    { 2, "Mid-term examinations will be held from November 1 to November 15, 2024. Students are advised to check the exam timetable on the notice board of their respective departments.", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mid-Term Examination Schedule" },
                    { 3, "Students are reminded that the deadline for fee submission for the current semester is October 31, 2024. Late submissions will incur a fine.", new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fee Submission Deadline" }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "CourseId", "DateOfBirth", "DepartmentId", "Education", "Email", "EnrollmentDate", "Fees", "Name", "Phone", "RollNumber" },
                values: new object[] { null, 1, new DateTime(2004, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Intermediate", "saad@student.nedacademy.edu", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50000m, "Saad Ahmed", "0311-1234567", "CS-2024-001" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "CourseId", "DateOfBirth", "DepartmentId", "Education", "Email", "EnrollmentDate", "Fees", "Name", "Phone", "RollNumber" },
                values: new object[] { null, 2, new DateTime(2003, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Graduate", "umar@student.nedacademy.edu", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35000m, "Umar Farooq", "0312-2345678", "CS-2024-002" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Code", "Credits", "DepartmentId", "Description", "Duration", "Name" },
                values: new object[,]
                {
                    { 3, "BEE", 4, 2, "4-year undergraduate degree in Electrical Engineering", 48, "Bachelor of Electrical Engineering" },
                    { 4, "BBA", 3, 3, "3-year undergraduate degree in Business Administration", 36, "Bachelor of Business Administration" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Address", "DepartmentId", "Email", "Gender", "ImageUrl", "JoiningDate", "Name", "Phone", "Qualification", "Salary" },
                values: new object[,]
                {
                    { 1, null, 1, "tasbeel@nedacademy.edu", "Male", null, new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sir Tasbeel Ahmad", "0300-1234567", "PhD Computer Science", 85000m },
                    { 2, null, 1, "mubashir@nedacademy.edu", "Male", null, new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sir Mubashir Hussain", "0301-2345678", "MS Networking & Security", 75000m },
                    { 3, null, 2, "sara@nedacademy.edu", "Female", null, new DateTime(2018, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dr. Sara Ali", "0302-3456789", "PhD Electrical Engineering", 95000m },
                    { 4, null, 3, "zubair@nedacademy.edu", "Male", null, new DateTime(2021, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prof. Zubair Sheikh", "0303-4567890", "MBA Finance", 80000m }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "CourseId", "DateOfBirth", "DepartmentId", "Education", "Email", "EnrollmentDate", "Fees", "Gender", "ImageUrl", "Name", "Phone", "RollNumber" },
                values: new object[,]
                {
                    { 3, null, 3, new DateTime(2004, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Intermediate", "fatima@student.nedacademy.edu", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55000m, "Female", null, "Fatima Khan", "0313-3456789", "EE-2024-001" },
                    { 4, null, 4, new DateTime(2005, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Intermediate", "ali@student.nedacademy.edu", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40000m, "Male", null, "Ali Hassan", "0314-4567890", "BBA-2024-001" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Students_CourseId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_DepartmentId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RollNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Students",
                newName: "FacultyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<double>(
                name: "Fees",
                table: "Students",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseModelId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacultyModelId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Networking");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "IoT");

            migrationBuilder.InsertData(
                table: "Faculty",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sir Tasbeel" },
                    { 2, "Sir mubashir" }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Age", "Course", "CourseId", "CourseModelId", "Education", "Faculty", "FacultyId", "FacultyModelId", "Fees", "Name" },
                values: new object[] { 20, "CyberSecurity", 0, null, "Inter", null, 0, null, 50000.0, "Saad" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Age", "Course", "CourseId", "CourseModelId", "Education", "Faculty", "FacultyId", "FacultyModelId", "Fees", "Name" },
                values: new object[] { 21, "Master CS", 0, null, "graduate", null, 0, null, 500000.0, "Umar" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseModelId",
                table: "Students",
                column: "CourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyModelId",
                table: "Students",
                column: "FacultyModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseModelId",
                table: "Students",
                column: "CourseModelId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Faculty_FacultyModelId",
                table: "Students",
                column: "FacultyModelId",
                principalTable: "Faculty",
                principalColumn: "Id");
        }
    }
}
