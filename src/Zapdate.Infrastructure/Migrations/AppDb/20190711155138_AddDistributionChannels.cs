using Microsoft.EntityFrameworkCore.Migrations;

namespace Zapdate.Infrastructure.Migrations.AppDb
{
    public partial class AddDistributionChannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistributionChannels",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistributionChannels",
                table: "Projects");
        }
    }
}
