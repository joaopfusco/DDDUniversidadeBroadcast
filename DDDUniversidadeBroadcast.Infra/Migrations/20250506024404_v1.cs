using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDUniversidadeBroadcast.Infra.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aberto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Curso = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UltimaNotificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participantes_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Participantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Postagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Curtidas = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postagens_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Postagens_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seguidores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SegueId = table.Column<int>(type: "int", nullable: false),
                    SeguidoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguidores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguidores_Usuarios_SegueId",
                        column: x => x.SegueId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seguidores_Usuarios_SeguidoId",
                        column: x => x.SeguidoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Nome",
                table: "Eventos",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_EventoId_UsuarioId",
                table: "Participantes",
                columns: new[] { "EventoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_UsuarioId",
                table: "Participantes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Postagens_AutorId",
                table: "Postagens",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Postagens_EventoId",
                table: "Postagens",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_SegueId_SeguidoId",
                table: "Seguidores",
                columns: new[] { "SegueId", "SeguidoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_SeguidoId",
                table: "Seguidores",
                column: "SeguidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Nome",
                table: "Usuarios",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "Postagens");

            migrationBuilder.DropTable(
                name: "Seguidores");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
