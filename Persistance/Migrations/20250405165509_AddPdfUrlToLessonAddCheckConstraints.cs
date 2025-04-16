using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class AddPdfUrlToLessonAddCheckConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DropConstraintIfExists(migrationBuilder, "quizzes", "CK_Quiz_Type");
            DropConstraintIfExists(migrationBuilder, "lessonwords", "CK_LessonWord_Type");
            DropConstraintIfExists(migrationBuilder, "quizquestions", "CK_QuizQuestion_QuestionType");
            DropConstraintIfExists(migrationBuilder, "userwordprogress", "CK_UserWordProgress_QuestionType");

            migrationBuilder.Sql(@"
                UPDATE quizzes 
                SET Type = 'Nouns'
                WHERE Type IS NULL OR Type NOT IN ('Nouns', 'Grammar')");

            migrationBuilder.Sql(@"
                UPDATE lessonwords 
                SET Type = 'Keyword'
                WHERE Type IS NULL OR Type NOT IN ('Keyword', 'Additional')");

            migrationBuilder.Sql(@"
                UPDATE quizquestions 
                SET QuestionType = 'ImageChoice'
                WHERE QuestionType IS NULL OR QuestionType NOT IN (
                    'ImageChoice', 'AudioChoice', 'ImageAudioChoice', 
                    'Spelling', 'GrammarSelection', 'Pronunciation', 'AdvancedSurvey'
                )");

            migrationBuilder.Sql(@"
                UPDATE userwordprogress 
                SET QuestionType = 'ImageChoice'
                WHERE QuestionType IS NULL OR QuestionType NOT IN (
                    'ImageChoice', 'AudioChoice', 'ImageAudioChoice', 
                    'Spelling', 'GrammarSelection', 'Pronunciation', 'AdvancedSurvey'
                )");

            migrationBuilder.Sql(@"
                ALTER TABLE quizzes
                ADD CONSTRAINT CK_Quiz_Type
                CHECK (Type IN ('Nouns', 'Grammar'))");

            migrationBuilder.Sql(@"
                ALTER TABLE lessonwords
                ADD CONSTRAINT CK_LessonWord_Type
                CHECK (Type IN ('Keyword', 'Additional'))");

            migrationBuilder.Sql(@"
                ALTER TABLE quizquestions
                ADD CONSTRAINT CK_QuizQuestion_QuestionType
                CHECK (QuestionType IN (
                    'ImageChoice', 'AudioChoice', 'ImageAudioChoice', 
                    'Spelling', 'GrammarSelection', 'Pronunciation', 'AdvancedSurvey'
                ))");

            migrationBuilder.Sql(@"
                ALTER TABLE userwordprogress
                ADD CONSTRAINT CK_UserWordProgress_QuestionType
                CHECK (QuestionType IN (
                    'ImageChoice', 'AudioChoice', 'ImageAudioChoice', 
                    'Spelling', 'GrammarSelection', 'Pronunciation', 'AdvancedSurvey'
                ))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DropConstraintIfExists(migrationBuilder, "quizzes", "CK_Quiz_Type");
            DropConstraintIfExists(migrationBuilder, "lessonwords", "CK_LessonWord_Type");
            DropConstraintIfExists(migrationBuilder, "quizquestions", "CK_QuizQuestion_QuestionType");
            DropConstraintIfExists(migrationBuilder, "userwordprogress", "CK_UserWordProgress_QuestionType");
        }

        private void DropConstraintIfExists(MigrationBuilder migrationBuilder, string tableName, string constraintName)
        {
            migrationBuilder.Sql($@"
                SET @dbname = DATABASE();
                SET @tablename = '{tableName}';
                SET @constraintname = '{constraintName}';
                SET @preparedStatement = (SELECT IF(
                    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
                     WHERE CONSTRAINT_SCHEMA = @dbname 
                     AND TABLE_NAME = @tablename 
                     AND CONSTRAINT_NAME = @constraintname) > 0,
                    CONCAT('ALTER TABLE ', @tablename, ' DROP CONSTRAINT ', @constraintname),
                    'SELECT 1'
                ));
                PREPARE stmt FROM @preparedStatement;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;");
        }
    }
}