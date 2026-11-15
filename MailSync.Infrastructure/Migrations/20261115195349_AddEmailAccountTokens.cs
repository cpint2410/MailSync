using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAccountTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmailAccounts");

            migrationBuilder.DropColumn(
                name: "SyncCursor",
                table: "EmailAccounts");

            migrationBuilder.RenameColumn(
                name: "EncryptedToken",
                table: "EmailAccounts",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "EmailAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmailAccounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiresAt",
                table: "EmailAccounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "EmailAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmailAccounts");

            migrationBuilder.DropColumn(
                name: "TokenExpiresAt",
                table: "EmailAccounts");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "EmailAccounts",
                newName: "EncryptedToken");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmailAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SyncCursor",
                table: "EmailAccounts",
                type: "text",
                nullable: true);
        }
    }
}
