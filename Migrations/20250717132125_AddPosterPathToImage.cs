using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_image_upload.Migrations
{
    /// <inheritdoc />
    public partial class AddPosterPathToImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "posterPath",
                table: "Images",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "posterPath",
                table: "Images");
        }
    }
}
