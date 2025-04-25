using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddKidQuizFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "KidLessons",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "KidLessons");
        }
    }
}
