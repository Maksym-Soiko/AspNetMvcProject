﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserFieldInUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "UserInfos");
        }
    }
}
