using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactProBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class _006NullableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Images_ImageId",
                table: "Contacts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "Contacts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Images_ImageId",
                table: "Contacts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Images_ImageId",
                table: "Contacts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "Contacts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Images_ImageId",
                table: "Contacts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
