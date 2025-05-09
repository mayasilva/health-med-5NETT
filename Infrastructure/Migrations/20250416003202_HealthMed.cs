using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HealthMed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "medico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Crm = table.Column<string>(type: "VARCHAR(6)", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    Especialidade = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    ValorConsulta = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paciente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "agenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Hora = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    IdMedico = table.Column<int>(type: "INT", nullable: false),
                    IdPaciente = table.Column<int>(type: "INT", nullable: false),
                    Confirmado = table.Column<string>(type: "VARCHAR(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agenda_medico_Id",
                        column: x => x.Id,
                        principalTable: "medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_agenda_paciente_Id",
                        column: x => x.Id,
                        principalTable: "paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agenda");

            migrationBuilder.DropTable(
                name: "medico");

            migrationBuilder.DropTable(
                name: "paciente");
        }
    }
}
