using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace upload.Migrations
{
    /// <inheritdoc />
    public partial class CreateFileMetadatasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles");

            migrationBuilder.RenameTable(
                name: "UploadedFiles",
                newName: "FileMetadatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileMetadatas",
                table: "FileMetadatas",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileMetadatas",
                table: "FileMetadatas");

            migrationBuilder.RenameTable(
                name: "FileMetadatas",
                newName: "UploadedFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles",
                column: "Id");
        }
    }
}
