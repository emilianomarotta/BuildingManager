using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deleteBuildingInStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Buildings_BuildingId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_BuildingId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_BuildingId",
                table: "User",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Buildings_BuildingId",
                table: "User",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
