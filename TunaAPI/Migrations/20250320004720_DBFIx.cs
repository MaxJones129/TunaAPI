using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TunaAPI.Migrations
{
    /// <inheritdoc />
    public partial class DBFIx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampsiteTypes",
                table: "CampsiteTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campsites",
                table: "Campsites");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "Songs");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Artists");

            migrationBuilder.RenameTable(
                name: "CampsiteTypes",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "Campsites",
                newName: "SongGenres");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Songs",
                table: "Songs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongGenres",
                table: "SongGenres",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Songs",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongGenres",
                table: "SongGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.RenameTable(
                name: "Songs",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "SongGenres",
                newName: "Campsites");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "CampsiteTypes");

            migrationBuilder.RenameTable(
                name: "Artists",
                newName: "Reservations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campsites",
                table: "Campsites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampsiteTypes",
                table: "CampsiteTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");
        }
    }
}
