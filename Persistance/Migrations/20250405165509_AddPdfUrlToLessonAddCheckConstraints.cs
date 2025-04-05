using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfUrlToLessonAddCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                 name: "PdfUrl",
                 table: "Lessons",
                 nullable: true,
                 maxLength: 2000); 

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_PdfUrl",
                table: "Lessons",
                column: "PdfUrl");

            migrationBuilder.Sql(@"
            ALTER TABLE Quizzes
            ADD CONSTRAINT CK_Quiz_Type
            CHECK (Type IN ('Nouns', 'Grammar'))");

            migrationBuilder.Sql(@"
            ALTER TABLE LessonWords
            ADD CONSTRAINT CK_LessonWord_Type
            CHECK (Type IN ('Keyword', 'Additional'))");

            migrationBuilder.Sql(@"
            ALTER TABLE QuizQuestions
            ADD CONSTRAINT CK_QuizQuestion_QuestionType
            CHECK (QuestionType IN (
                'ImageChoice', 
                'AudioChoice',
                'ImageAudioChoice', 
                'Spelling',
                'GrammarSelection',
                'Pronunciation',
                'AdvancedSurvey'
            ))");

            migrationBuilder.Sql(@"
            ALTER TABLE UserWordProgresses
            ADD CONSTRAINT CK_UserWordProgress_QuestionType
            CHECK (QuestionType IN (
                'ImageChoice', 
                'AudioChoice',
                'ImageAudioChoice', 
                'Spelling',
                'GrammarSelection',
                'Pronunciation',
                'AdvancedSurvey'
            ))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Quizzes DROP CONSTRAINT CK_Quiz_Type");
            migrationBuilder.Sql("ALTER TABLE LessonWords DROP CONSTRAINT CK_LessonWord_Type");
            migrationBuilder.Sql("ALTER TABLE QuizQuestions DROP CONSTRAINT CK_QuizQuestion_QuestionType");
            migrationBuilder.Sql("ALTER TABLE UserWordProgresses DROP CONSTRAINT CK_UserWordProgress_QuestionType");
        }
    }
}
