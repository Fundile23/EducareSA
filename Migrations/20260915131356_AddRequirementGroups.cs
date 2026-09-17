using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducareSA.Migrations
{
    /// <inheritdoc />
    public partial class AddRequirementGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequirementGroup",
                table: "ProgrammeSubjectRequirements",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequirementGroup",
                table: "ProgrammeSubjectRequirements");
        }
    }
}
