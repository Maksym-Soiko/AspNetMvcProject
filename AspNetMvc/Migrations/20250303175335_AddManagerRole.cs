using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[,]
            {
               { Guid.NewGuid().ToString(), "Manager", "MANAGER" }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
