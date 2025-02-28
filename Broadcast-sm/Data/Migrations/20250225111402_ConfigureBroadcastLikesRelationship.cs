using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broadcast_sm.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureBroadcastLikesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Broadcasts_BroadcastId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BroadcastId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BroadcastId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "BroadcastLikes",
                columns: table => new
                {
                    BroadcastId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroadcastLikes", x => new { x.BroadcastId, x.UserId });
                    table.ForeignKey(
                        name: "FK_BroadcastLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BroadcastLikes_Broadcasts_BroadcastId",
                        column: x => x.BroadcastId,
                        principalTable: "Broadcasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BroadcastLikes_UserId",
                table: "BroadcastLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BroadcastLikes");

            migrationBuilder.AddColumn<int>(
                name: "BroadcastId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BroadcastId",
                table: "AspNetUsers",
                column: "BroadcastId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Broadcasts_BroadcastId",
                table: "AspNetUsers",
                column: "BroadcastId",
                principalTable: "Broadcasts",
                principalColumn: "Id");
        }
    }
}
