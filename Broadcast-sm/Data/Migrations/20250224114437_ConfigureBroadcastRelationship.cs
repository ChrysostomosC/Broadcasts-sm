using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broadcast_sm.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureBroadcastRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Broadcasts_AspNetUsers_UserId",
                table: "Broadcasts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Broadcasts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BroadcastId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserListening",
                columns: table => new
                {
                    ListeningToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListening", x => new { x.ListeningToId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserListening_AspNetUsers_ListeningToId",
                        column: x => x.ListeningToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserListening_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BroadcastId",
                table: "AspNetUsers",
                column: "BroadcastId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListening_UserId",
                table: "UserListening",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Broadcasts_BroadcastId",
                table: "AspNetUsers",
                column: "BroadcastId",
                principalTable: "Broadcasts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Broadcasts_AspNetUsers_UserId",
                table: "Broadcasts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Broadcasts_BroadcastId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Broadcasts_AspNetUsers_UserId",
                table: "Broadcasts");

            migrationBuilder.DropTable(
                name: "UserListening");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BroadcastId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BroadcastId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Broadcasts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Broadcasts_AspNetUsers_UserId",
                table: "Broadcasts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
