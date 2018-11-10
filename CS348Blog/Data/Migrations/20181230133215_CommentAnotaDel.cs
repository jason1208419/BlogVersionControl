using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CS348Blog.Data.Migrations
{
    public partial class CommentAnotaDel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsUnderComment");

            migrationBuilder.DropTable(
                name: "CommentsUnderPost");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "MainComments",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    PostID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainComments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_MainComments_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubComments",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    UnderCommentID = table.Column<int>(nullable: false),
                    MainCommentCommentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubComments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_SubComments_MainComments_MainCommentCommentID",
                        column: x => x.MainCommentCommentID,
                        principalTable: "MainComments",
                        principalColumn: "CommentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainComments_PostID",
                table: "MainComments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_SubComments_MainCommentCommentID",
                table: "SubComments",
                column: "MainCommentCommentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubComments");

            migrationBuilder.DropTable(
                name: "MainComments");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CommentsUnderPost",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    PostID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsUnderPost", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_CommentsUnderPost_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentsUnderComment",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommentUnderPostCommentID = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    UnderCommentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsUnderComment", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_CommentsUnderComment_CommentsUnderPost_CommentUnderPostCommentID",
                        column: x => x.CommentUnderPostCommentID,
                        principalTable: "CommentsUnderPost",
                        principalColumn: "CommentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentsUnderComment_CommentUnderPostCommentID",
                table: "CommentsUnderComment",
                column: "CommentUnderPostCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsUnderPost_PostID",
                table: "CommentsUnderPost",
                column: "PostID");
        }
    }
}
