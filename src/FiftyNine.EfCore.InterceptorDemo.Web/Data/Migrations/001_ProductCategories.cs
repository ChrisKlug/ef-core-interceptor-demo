using Microsoft.EntityFrameworkCore.Migrations;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

[Migration("001_ProductCategories")]
[Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(DemoDbContext))]
public class _001_ProductCategories : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProductCategories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(128)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductCategories", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProductCategories"
        );
    }
}
