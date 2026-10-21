using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NedAcademy.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationNo = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GuardianName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Cnic = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreviousEducation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PassingYear = table.Column<int>(type: "int", nullable: true),
                    MarksPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PreviousInstitute = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChallanNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnrolledStudentId = table.Column<int>(type: "int", nullable: true),
                    GeneratedRollNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Students_EnrolledStudentId",
                        column: x => x.EnrolledStudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Admissions",
                columns: new[] { "Id", "Address", "AdminRemarks", "ApplicationNo", "AppliedDate", "ChallanNo", "Cnic", "CourseId", "DateOfBirth", "DepartmentId", "DocumentUrl", "Email", "EnrolledStudentId", "FeeAmount", "FeeStatus", "FullName", "Gender", "GeneratedRollNumber", "GuardianName", "MarksPercentage", "PassingYear", "PaymentDate", "Phone", "PreviousEducation", "PreviousInstitute", "Status" },
                values: new object[,]
                {
                    { 1, "Gulshan-e-Iqbal, Block 6, Karachi", "New online application submitted. Awaiting document verification.", "NED-2026-001", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "42101-1234567-2", 1, new DateTime(2005, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "zainab.tariq@gmail.com", null, 65000m, "Unpaid", "Zainab Tariq", "Female", null, "Tariq Mehmood", 84.50m, 2025, null, "0321-9876543", "HSC Pre-Engineering", "Aga Khan Higher Secondary School", 0 },
                    { 2, "North Nazimabad, Karachi", "Documents under review. Marks meet minimum eligibility criteria.", "NED-2026-002", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "42201-9876543-1", 1, new DateTime(2004, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "hamza.bilal@outlook.com", null, 65000m, "Unpaid", "Hamza Bilal", "Male", null, "Bilal Ahmed", 76.20m, 2025, null, "0333-5551234", "ICS Computer Science", "DJ Sindh Govt Science College", 1 },
                    { 3, "PECHS Block 2, Karachi", "Shortlisted for admission offer. Fee voucher generated and emailed.", "NED-2026-003", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "42301-4455667-4", 3, new DateTime(2005, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "ayesha.s@yahoo.com", null, 70000m, "Unpaid", "Ayesha Siddiqui", "Female", null, "Rashid Siddiqui", 88.00m, 2025, null, "0345-1122334", "A-Levels Science", "The City School PAF Chapter", 2 },
                    { 4, "Gulistan-e-Jauhar, Karachi", "Fee payment verified via bank voucher. Ready for formal student enrollment.", "NED-2026-004", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "CHL-2026-8841", "42101-7788991-3", 1, new DateTime(2004, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "bilal.munir@gmail.com", null, 65000m, "Paid", "Bilal Munir", "Male", null, "Munir Akhtar", 81.30m, 2025, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "0315-7788990", "HSC Pre-Engineering", "Adamjee Govt Science College", 3 },
                    { 5, "Malir Cantt, Karachi", "Application rejected: Minimum 50% marks required for BBA program.", "NED-2026-005", new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "42501-1122334-5", 4, new DateTime(2003, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "hassan.raza@gmail.com", null, 50000m, "Unpaid", "Hassan Raza", "Male", null, "Raza Ali", 44.00m, 2024, null, "0300-3344556", "HSC Commerce", "Govt National College", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_CourseId",
                table: "Admissions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DepartmentId",
                table: "Admissions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_EnrolledStudentId",
                table: "Admissions",
                column: "EnrolledStudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admissions");
        }
    }
}
