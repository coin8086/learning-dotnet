using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFModelGeneratedValues.Migrations
{
    /// <inheritdoc />
    public partial class MakeInternalNameNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InternalName",
                table: "Blogs",
                type: "TEXT",
                nullable: false,
                computedColumnSql: "[Id] || '-' || [Name]",
                stored: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComputedColumnSql: "[Id] || '-' || [Name]",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InternalName",
                table: "Blogs",
                type: "TEXT",
                nullable: true,
                computedColumnSql: "[Id] || '-' || [Name]",
                stored: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComputedColumnSql: "[Id] || '-' || [Name]",
                oldStored: true);
        }
    }
}
