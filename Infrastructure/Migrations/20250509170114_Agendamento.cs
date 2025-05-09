using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Agendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agendamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    IdAgenda = table.Column<int>(type: "INT", nullable: false),
                    IdPaciente = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agendamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agendamento_agenda_IdAgenda",
                        column: x => x.IdAgenda,
                        principalTable: "agenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_agendamento_paciente_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_agendamento_IdAgenda",
                table: "agendamento",
                column: "IdAgenda");

            migrationBuilder.CreateIndex(
                name: "IX_agendamento_IdPaciente",
                table: "agendamento",
                column: "IdPaciente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agendamento");
        }
    }
}
