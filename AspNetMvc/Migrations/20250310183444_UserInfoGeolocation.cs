using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetMvc.Migrations
{
    /// <inheritdoc />
    public partial class UserInfoGeolocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Lat",
                table: "UserInfos",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Lng",
                table: "UserInfos",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "UserInfos");
        }
    }
}
