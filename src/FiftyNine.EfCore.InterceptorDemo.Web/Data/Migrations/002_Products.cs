using Microsoft.EntityFrameworkCore.Migrations;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

[Migration("002_Products")]
[Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(DemoDbContext))]
public class _002_Products : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ProductCategoryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey(
                    name: "FK_Products_ProductCategories_ProductCategoryId",
                    column: x => x.ProductCategoryId,
                    principalTable: "ProductCategories",
                    principalColumn: "Id");
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Products"
        );
    }
}