using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MeusPlanos.Modelo.Migrations
{
    /// <inheritdoc />
    public partial class InicialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Papel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CodigoRedefinicaoSenha = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plano",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    ValorEstimadoManual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataAlvo = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Moeda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProprietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plano", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plano_Usuario_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPapel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PapelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPapel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioPapel_Papel_PapelId",
                        column: x => x.PapelId,
                        principalTable: "Papel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPapel_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Papel",
                columns: new[] { "Id", "DataAlteracao", "DataCriacao", "DataDelecao", "Nome", "Observacao" },
                values: new object[,]
                {
                    { new Guid("7e0a9201-f672-11ed-9ae1-0fc4e648d876"), null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Admin", null },
                    { new Guid("7e0a9202-f672-11ed-9ae1-0fc4e648d876"), null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "SuperAdmin", null }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "CodigoRedefinicaoSenha", "DataAlteracao", "DataDelecao", "Email", "Nome", "PasswordHash", "Sobrenome", "Username" },
                values: new object[] { new Guid("7e0a9200-f672-11ed-9ae1-0fc4e648d876"), null, null, null, "admin@admin.com", "Administrador", "AITMdMEsqiixw35g6qbq+zbaYM2HttO8uXFdJSg96xVnUn2AatqTCDcKqSVPlbzulA==", "do Sistema", "admin" });

            migrationBuilder.InsertData(
                table: "UsuarioPapel",
                columns: new[] { "Id", "DataAlteracao", "DataDelecao", "PapelId", "UsuarioId" },
                values: new object[,]
                {
                    { new Guid("7e0a9203-f672-11ed-9ae1-0fc4e648d876"), null, null, new Guid("7e0a9201-f672-11ed-9ae1-0fc4e648d876"), new Guid("7e0a9200-f672-11ed-9ae1-0fc4e648d876") },
                    { new Guid("7e0a9204-f672-11ed-9ae1-0fc4e648d876"), null, null, new Guid("7e0a9202-f672-11ed-9ae1-0fc4e648d876"), new Guid("7e0a9200-f672-11ed-9ae1-0fc4e648d876") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Papel_Nome",
                table: "Papel",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plano_ProprietarioId",
                table: "Plano",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Username",
                table: "Usuario",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPapel_PapelId",
                table: "UsuarioPapel",
                column: "PapelId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPapel_UsuarioId",
                table: "UsuarioPapel",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plano");

            migrationBuilder.DropTable(
                name: "UsuarioPapel");

            migrationBuilder.DropTable(
                name: "Papel");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
