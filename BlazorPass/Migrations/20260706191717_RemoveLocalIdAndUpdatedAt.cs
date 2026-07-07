using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorPass.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLocalIdAndUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_training_history_user_local_id",
                table: "training_history");

            migrationBuilder.DropColumn(
                name: "local_id",
                table: "training_history");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "training_history");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "training_history",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "idx_training_history_user_client_id",
                table: "training_history",
                columns: new[] { "user_id", "client_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_training_history_user_client_id",
                table: "training_history");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "training_history",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "local_id",
                table: "training_history",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "training_history",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "idx_training_history_user_local_id",
                table: "training_history",
                columns: new[] { "user_id", "local_id" },
                unique: true);
        }
    }
}
