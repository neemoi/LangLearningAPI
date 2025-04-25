using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistance.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddKidQuizModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KidLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidLessons", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KidQuizTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidQuizTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KidWordCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    Word = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AudioUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidWordCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidWordCards_KidLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "KidLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KidQuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    WordCardId = table.Column<int>(type: "int", nullable: false),
                    QuizTypeId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AudioUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CorrectAnswer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidQuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidQuizQuestions_KidLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "KidLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidQuizQuestions_KidQuizTypes_QuizTypeId",
                        column: x => x.QuizTypeId,
                        principalTable: "KidQuizTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidQuizQuestions_KidWordCards_WordCardId",
                        column: x => x.WordCardId,
                        principalTable: "KidWordCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KidQuizAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCorrect = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidQuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidQuizAnswers_KidQuizQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "KidQuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "KidQuizTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "image_choice" },
                    { 2, "audio_choice" },
                    { 3, "image_audio_choice" },
                    { 4, "spelling" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KidQuizAnswers_QuestionId",
                table: "KidQuizAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_KidQuizQuestions_LessonId",
                table: "KidQuizQuestions",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_KidQuizQuestions_QuizTypeId",
                table: "KidQuizQuestions",
                column: "QuizTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KidQuizQuestions_WordCardId",
                table: "KidQuizQuestions",
                column: "WordCardId");

            migrationBuilder.CreateIndex(
                name: "IX_KidWordCards_LessonId",
                table: "KidWordCards",
                column: "LessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KidQuizAnswers");

            migrationBuilder.DropTable(
                name: "KidQuizQuestions");

            migrationBuilder.DropTable(
                name: "KidQuizTypes");

            migrationBuilder.DropTable(
                name: "KidWordCards");

            migrationBuilder.DropTable(
                name: "KidLessons");
        }
    }
}
