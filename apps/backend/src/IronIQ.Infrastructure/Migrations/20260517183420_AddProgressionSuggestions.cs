using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressionSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgressionSuggestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentWeightKg = table.Column<float>(type: "real", nullable: false),
                    SuggestedWeightKg = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressionSuggestions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressionSuggestions_SessionId",
                table: "ProgressionSuggestions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressionSuggestions_UserId",
                table: "ProgressionSuggestions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgressionSuggestions");
        }
    }
}
