using Microsoft.EntityFrameworkCore.Migrations;

namespace Jeopardy.Data.Migrations
{
    public partial class AnswerTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenRead",
                schema: "dbo",
                table: "Answers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenRead",
                schema: "dbo",
                table: "Answers");
        }
    }
}
