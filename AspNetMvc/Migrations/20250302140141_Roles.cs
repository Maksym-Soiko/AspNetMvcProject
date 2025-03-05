using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetMvc.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[,]
            {
                { Guid.NewGuid().ToString(), "Admin", "ADMIN" },
                { Guid.NewGuid().ToString(), "User", "USER" }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
