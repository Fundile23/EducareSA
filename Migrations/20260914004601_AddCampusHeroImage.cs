using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducareSA.Migrations
{
    /// <inheritdoc />
    public partial class AddCampusHeroImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Campuses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Campuses");
        }
    }
}
