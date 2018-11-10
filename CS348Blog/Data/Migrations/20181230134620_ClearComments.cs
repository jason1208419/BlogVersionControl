using Microsoft.EntityFrameworkCore.Migrations;

namespace CS348Blog.Data.Migrations
{
    public partial class ClearComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubComments_MainComments_MainCommentCommentID",
                table: "SubComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubComments",
                table: "SubComments");

            migrationBuilder.RenameTable(
                name: "SubComments",
                newName: "SubCommentssss");

            migrationBuilder.RenameIndex(
                name: "IX_SubComments_MainCommentCommentID",
                table: "SubCommentssss",
                newName: "IX_SubCommentssss_MainCommentCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCommentssss",
                table: "SubCommentssss",
                column: "CommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCommentssss_MainComments_MainCommentCommentID",
                table: "SubCommentssss",
                column: "MainCommentCommentID",
                principalTable: "MainComments",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCommentssss_MainComments_MainCommentCommentID",
                table: "SubCommentssss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCommentssss",
                table: "SubCommentssss");

            migrationBuilder.RenameTable(
                name: "SubCommentssss",
                newName: "SubComments");

            migrationBuilder.RenameIndex(
                name: "IX_SubCommentssss_MainCommentCommentID",
                table: "SubComments",
                newName: "IX_SubComments_MainCommentCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubComments",
                table: "SubComments",
                column: "CommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubComments_MainComments_MainCommentCommentID",
                table: "SubComments",
                column: "MainCommentCommentID",
                principalTable: "MainComments",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
