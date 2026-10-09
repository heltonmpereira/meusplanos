using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeusPlanos.Modelo.Migrations
{
    /// <inheritdoc />
    public partial class PlanoItemMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanoItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ValorOrcamentoEstimado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoItem_Plano_PlanoId",
                        column: x => x.PlanoId,
                        principalTable: "Plano",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanoItemOpcao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanoItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Caracteristicas = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoItemOpcao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoItemOpcao_PlanoItem_PlanoItemId",
                        column: x => x.PlanoItemId,
                        principalTable: "PlanoItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanoItemOpcaoLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanoItemOpcaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NomeSite = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CondicoesPagamento = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DataCotacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoItemOpcaoLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoItemOpcaoLink_PlanoItemOpcao_PlanoItemOpcaoId",
                        column: x => x.PlanoItemOpcaoId,
                        principalTable: "PlanoItemOpcao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItem_PlanoId",
                table: "PlanoItem",
                column: "PlanoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcao_PlanoItemId",
                table: "PlanoItemOpcao",
                column: "PlanoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcaoLink_PlanoItemOpcaoId",
                table: "PlanoItemOpcaoLink",
                column: "PlanoItemOpcaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanoItemOpcaoLink");

            migrationBuilder.DropTable(
                name: "PlanoItemOpcao");

            migrationBuilder.DropTable(
                name: "PlanoItem");
        }
    }
}
