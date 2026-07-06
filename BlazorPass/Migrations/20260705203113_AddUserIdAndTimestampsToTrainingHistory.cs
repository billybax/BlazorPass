using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorPass.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdAndTimestampsToTrainingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_training_history_updated_at",
                table: "training_history");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "training_history",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "client_updated_at",
                table: "training_history",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<int>(
                name: "local_id",
                table: "training_history",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "server_updated_at",
                table: "training_history",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "training_history",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "idx_training_history_server_updated_at",
                table: "training_history",
                column: "server_updated_at");

            migrationBuilder.CreateIndex(
                name: "idx_training_history_user_id",
                table: "training_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_training_history_user_local_id",
                table: "training_history",
                columns: new[] { "user_id", "local_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_training_history_server_updated_at",
                table: "training_history");

            migrationBuilder.DropIndex(
                name: "idx_training_history_user_id",
                table: "training_history");

            migrationBuilder.DropIndex(
                name: "idx_training_history_user_local_id",
                table: "training_history");

            migrationBuilder.DropColumn(
                name: "client_updated_at",
                table: "training_history");

            migrationBuilder.DropColumn(
                name: "local_id",
                table: "training_history");

            migrationBuilder.DropColumn(
                name: "server_updated_at",
                table: "training_history");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "training_history");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "training_history",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "idx_training_history_updated_at",
                table: "training_history",
                column: "updated_at");
        }
    }
}
