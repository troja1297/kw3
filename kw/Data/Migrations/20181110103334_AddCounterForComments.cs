using Microsoft.EntityFrameworkCore.Migrations;

namespace kw.Data.Migrations
{
    public partial class AddCounterForComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "ThemeModel",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentCount",
                table: "ThemeModel");
        }
    }
}
