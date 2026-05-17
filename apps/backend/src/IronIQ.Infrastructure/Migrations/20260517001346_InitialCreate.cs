using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CoinBalance = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionTier = table.Column<string>(type: "text", nullable: false),
                    Profile_Name = table.Column<string>(type: "text", nullable: true),
                    Profile_Age = table.Column<int>(type: "integer", nullable: true),
                    Profile_HeightCm = table.Column<float>(type: "real", nullable: true),
                    Profile_WeightKg = table.Column<float>(type: "real", nullable: true),
                    Profile_Goal = table.Column<string>(type: "text", nullable: true),
                    Profile_Level = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenHash = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
