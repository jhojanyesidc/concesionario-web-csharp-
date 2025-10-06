using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcesionarioWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiculoAndConductor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conductor",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tipo_licencia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_tipo_conductor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conductor", x => x.id);
                    table.ForeignKey(
                        name: "FK_conductor_tipo_conductor_id_tipo_conductor",
                        column: x => x.id_tipo_conductor,
                        principalSchema: "dbo",
                        principalTable: "tipo_conductor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehiculo",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    matricula = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    año = table.Column<int>(type: "int", nullable: false),
                    id_tipo_vehiculo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculo", x => x.id);
                    table.ForeignKey(
                        name: "FK_vehiculo_tipo_vehiculo_id_tipo_vehiculo",
                        column: x => x.id_tipo_vehiculo,
                        principalSchema: "dbo",
                        principalTable: "tipo_vehiculo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_conductor_id_tipo_conductor",
                schema: "dbo",
                table: "conductor",
                column: "id_tipo_conductor");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculo_id_tipo_vehiculo",
                schema: "dbo",
                table: "vehiculo",
                column: "id_tipo_vehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conductor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "vehiculo",
                schema: "dbo");
        }
    }
}
