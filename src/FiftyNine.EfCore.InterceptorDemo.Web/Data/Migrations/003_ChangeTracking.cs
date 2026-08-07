using Microsoft.EntityFrameworkCore.Migrations;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

[Migration("003_ChangeTracking")]
[Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(DemoDbContext))]
public class _003_ChangeTracking : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "LastModifiedAt",
            table: "Products",
            type: "datetimeoffset",
            nullable: false,
            defaultValue: DateTimeOffset.UtcNow);

        migrationBuilder.AddColumn<string>(
            name: "LastModifiedBy",
            table: "Products",
            type: "nvarchar(128)",
            nullable: false,
            defaultValue: "System");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "LastModifiedAt",
            table: "Products"
        );

        migrationBuilder.DropColumn(
            name: "LastModifiedBy",
            table: "Products"
        );
    }
}