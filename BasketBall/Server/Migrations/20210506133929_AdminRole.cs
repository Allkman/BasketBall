using Microsoft.EntityFrameworkCore.Migrations;

namespace BasketBall.Server.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO AspNetRoles (Id, [Name], NormalizedName)
VALUES('da519098-ec12-4534-b449-2fed153bd757','Admin', 'Admin')"
);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
