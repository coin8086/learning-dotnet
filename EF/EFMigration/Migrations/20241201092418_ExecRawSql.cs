using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigration.Migrations
{
    /// <inheritdoc />
    public partial class ExecRawSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
INSERT INTO Blogs (Name)
VALUES 
(""Blog 1""),
(""Blog 2""),
(""Blog 3"");
";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"
DELETE FROM Blogs
WHERE Name in (""Blog 1"", ""Blog 2"", ""Blog 3"");
";
            migrationBuilder.Sql(sql);
        }
    }
}
