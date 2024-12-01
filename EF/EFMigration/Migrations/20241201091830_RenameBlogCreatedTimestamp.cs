using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigration.Migrations
{
    /// <inheritdoc />
    public partial class RenameBlogCreatedTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTimestamp",
                table: "Blogs",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Blogs",
                newName: "CreatedTimestamp");
        }
    }
}
