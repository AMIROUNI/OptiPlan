﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiPlanBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixSprintFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sprints",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sprints");
        }
    }
}
