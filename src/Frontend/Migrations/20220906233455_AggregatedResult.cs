using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frontend.Migrations
{
    public partial class AggregatedResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "JobResult",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "JobResult");
        }
    }
}
