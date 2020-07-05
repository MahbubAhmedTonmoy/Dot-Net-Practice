using Microsoft.EntityFrameworkCore.Migrations;

namespace SeliseExam.Migrations
{
    public partial class demochange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Demo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Demo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Demo");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Demo");
        }
    }
}
