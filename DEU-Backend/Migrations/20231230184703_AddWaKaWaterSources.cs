using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEU_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddWaKaWaterSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaKaWaterSources",
                columns: table => new
                {
                    WaKaWaterSourceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IconWidth = table.Column<double>(type: "REAL", nullable: false),
                    IconHeight = table.Column<double>(type: "REAL", nullable: false),
                    IconAnchorX = table.Column<double>(type: "REAL", nullable: false),
                    IconAnchorY = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Flowrate = table.Column<int>(type: "INTEGER", nullable: false),
                    Connections = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaKaWaterSources", x => x.WaKaWaterSourceId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaKaWaterSources");
        }
    }
}
