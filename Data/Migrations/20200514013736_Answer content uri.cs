using Microsoft.EntityFrameworkCore.Migrations;

namespace Jeopardy.Data.Migrations
{
    public partial class Answercontenturi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentUri",
                schema: "dbo",
                table: "Answers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentUri",
                schema: "dbo",
                table: "Answers");
        }
    }
}
