using Microsoft.EntityFrameworkCore.Migrations;

namespace CS348Blog.Data.Migrations
{
    public partial class SubComUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubComments_MainComments_MainCommentCommentID",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "UnderCommentID",
                table: "SubComments");

            migrationBuilder.AlterColumn<int>(
                name: "MainCommentCommentID",
                table: "SubComments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubComments_MainComments_MainCommentCommentID",
                table: "SubComments",
                column: "MainCommentCommentID",
                principalTable: "MainComments",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubComments_MainComments_MainCommentCommentID",
                table: "SubComments");

            migrationBuilder.AlterColumn<int>(
                name: "MainCommentCommentID",
                table: "SubComments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UnderCommentID",
                table: "SubComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SubComments_MainComments_MainCommentCommentID",
                table: "SubComments",
                column: "MainCommentCommentID",
                principalTable: "MainComments",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
