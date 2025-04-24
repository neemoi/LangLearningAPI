using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class EnglishNameFieldsModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MaleNames",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FemaleNames",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MaleNames");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FemaleNames");
        }
    }
}
