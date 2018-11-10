using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CS348Blog.Data.Migrations
{
    public partial class CommentCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentUnderPost",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    PostID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUnderPost", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_CommentUnderPost_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentUnderComment",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    UnderCommentID = table.Column<int>(nullable: false),
                    CommentUnderPostCommentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUnderComment", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_CommentUnderComment_CommentUnderPost_CommentUnderPostCommentID",
                        column: x => x.CommentUnderPostCommentID,
                        principalTable: "CommentUnderPost",
                        principalColumn: "CommentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentUnderComment_CommentUnderPostCommentID",
                table: "CommentUnderComment",
                column: "CommentUnderPostCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUnderPost_PostID",
                table: "CommentUnderPost",
                column: "PostID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentUnderComment");

            migrationBuilder.DropTable(
                name: "CommentUnderPost");
        }
    }
}
