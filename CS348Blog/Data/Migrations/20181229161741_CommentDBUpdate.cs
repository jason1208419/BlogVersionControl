using Microsoft.EntityFrameworkCore.Migrations;

namespace CS348Blog.Data.Migrations
{
    public partial class CommentDBUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentUnderComment_CommentUnderPost_CommentUnderPostCommentID",
                table: "CommentUnderComment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentUnderPost_Posts_PostID",
                table: "CommentUnderPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentUnderPost",
                table: "CommentUnderPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentUnderComment",
                table: "CommentUnderComment");

            migrationBuilder.RenameTable(
                name: "CommentUnderPost",
                newName: "CommentsUnderPost");

            migrationBuilder.RenameTable(
                name: "CommentUnderComment",
                newName: "CommentsUnderComment");

            migrationBuilder.RenameIndex(
                name: "IX_CommentUnderPost_PostID",
                table: "CommentsUnderPost",
                newName: "IX_CommentsUnderPost_PostID");

            migrationBuilder.RenameIndex(
                name: "IX_CommentUnderComment_CommentUnderPostCommentID",
                table: "CommentsUnderComment",
                newName: "IX_CommentsUnderComment_CommentUnderPostCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentsUnderPost",
                table: "CommentsUnderPost",
                column: "CommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentsUnderComment",
                table: "CommentsUnderComment",
                column: "CommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsUnderComment_CommentsUnderPost_CommentUnderPostCommentID",
                table: "CommentsUnderComment",
                column: "CommentUnderPostCommentID",
                principalTable: "CommentsUnderPost",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsUnderPost_Posts_PostID",
                table: "CommentsUnderPost",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "PostID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentsUnderComment_CommentsUnderPost_CommentUnderPostCommentID",
                table: "CommentsUnderComment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsUnderPost_Posts_PostID",
                table: "CommentsUnderPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentsUnderPost",
                table: "CommentsUnderPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentsUnderComment",
                table: "CommentsUnderComment");

            migrationBuilder.RenameTable(
                name: "CommentsUnderPost",
                newName: "CommentUnderPost");

            migrationBuilder.RenameTable(
                name: "CommentsUnderComment",
                newName: "CommentUnderComment");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsUnderPost_PostID",
                table: "CommentUnderPost",
                newName: "IX_CommentUnderPost_PostID");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsUnderComment_CommentUnderPostCommentID",
                table: "CommentUnderComment",
                newName: "IX_CommentUnderComment_CommentUnderPostCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentUnderPost",
                table: "CommentUnderPost",
                column: "CommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentUnderComment",
                table: "CommentUnderComment",
                column: "CommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUnderComment_CommentUnderPost_CommentUnderPostCommentID",
                table: "CommentUnderComment",
                column: "CommentUnderPostCommentID",
                principalTable: "CommentUnderPost",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUnderPost_Posts_PostID",
                table: "CommentUnderPost",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "PostID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
