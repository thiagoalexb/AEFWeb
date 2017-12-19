using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AEFWeb.Data.Migrations
{
    public partial class configss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Events_EventId",
                table: "Lessons");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "EventLog",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "EventLog",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Events_EventId",
                table: "Lessons",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Events_EventId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "EventLog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "EventLog",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Events_EventId",
                table: "Lessons",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
