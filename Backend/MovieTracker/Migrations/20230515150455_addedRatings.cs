using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTracker.Migrations
{
    /// <inheritdoc />
    public partial class addedRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AppUserId",
                table: "Ratings",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_AppUserId",
                table: "Ratings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_AppUserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AppUserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Ratings");
        }
    }
}
