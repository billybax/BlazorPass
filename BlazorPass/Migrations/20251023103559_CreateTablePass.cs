using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlazorPass.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablePass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // LocPass table already exists in the database
            // This migration documents the schema mapping for Entity Framework Core
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Table already exists, no rollback needed
        }
    }
}
