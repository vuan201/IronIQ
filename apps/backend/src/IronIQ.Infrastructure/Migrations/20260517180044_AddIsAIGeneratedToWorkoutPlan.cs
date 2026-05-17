using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAIGeneratedToWorkoutPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAIGenerated",
                table: "WorkoutPlans",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAIGenerated",
                table: "WorkoutPlans");
        }
    }
}
