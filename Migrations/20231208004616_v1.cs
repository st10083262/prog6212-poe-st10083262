using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyTracker.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    moduleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    moduleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    credits = table.Column<int>(type: "int", nullable: false),
                    classHoursPerWeek = table.Column<double>(type: "float", nullable: false),
                    semesterNumOfWeeks = table.Column<int>(type: "int", nullable: false),
                    semesterStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.moduleCode);
                    table.ForeignKey(
                        name: "FK_Module_User_username",
                        column: x => x.username,
                        principalTable: "User",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyDate",
                columns: table => new
                {
                    studyDateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hoursStudied = table.Column<double>(type: "float", nullable: false),
                    moduleCode = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyDate", x => x.studyDateID);
                    table.ForeignKey(
                        name: "FK_StudyDate_Module_moduleCode",
                        column: x => x.moduleCode,
                        principalTable: "Module",
                        principalColumn: "moduleCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Module_username",
                table: "Module",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_StudyDate_moduleCode",
                table: "StudyDate",
                column: "moduleCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyDate");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
