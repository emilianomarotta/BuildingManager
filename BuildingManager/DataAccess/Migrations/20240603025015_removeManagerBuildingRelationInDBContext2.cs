using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeManagerBuildingRelationInDBContext2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_User_ManagerId",
                table: "Buildings");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_User_ManagerId",
                table: "Buildings",
                column: "ManagerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_User_ManagerId",
                table: "Buildings");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_User_ManagerId",
                table: "Buildings",
                column: "ManagerId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
