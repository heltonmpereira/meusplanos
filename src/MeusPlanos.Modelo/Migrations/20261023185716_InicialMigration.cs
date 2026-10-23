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
                name: "Imagem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeOriginal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NomeArquivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CaminhoRelativo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagem", x => x.Id);
                });

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
                    ProprietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagemCapaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plano", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plano_Imagem_ImagemCapaId",
                        column: x => x.ImagemCapaId,
                        principalTable: "Imagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    ImagemCapaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoItem_Imagem_ImagemCapaId",
                        column: x => x.ImagemCapaId,
                        principalTable: "Imagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanoItem_Plano_PlanoId",
                        column: x => x.PlanoId,
                        principalTable: "Plano",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ImagemCapaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoItemOpcao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoItemOpcao_Imagem_ImagemCapaId",
                        column: x => x.ImagemCapaId,
                        principalTable: "Imagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanoItemOpcao_PlanoItem_PlanoItemId",
                        column: x => x.PlanoItemId,
                        principalTable: "PlanoItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanoItemOpcaoLinkImagem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanoItemOpcaoLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    EhCapa = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDelecao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoItemOpcaoLinkImagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoItemOpcaoLinkImagem_Imagem_ImagemId",
                        column: x => x.ImagemId,
                        principalTable: "Imagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanoItemOpcaoLinkImagem_PlanoItemOpcaoLink_PlanoItemOpcaoLinkId",
                        column: x => x.PlanoItemOpcaoLinkId,
                        principalTable: "PlanoItemOpcaoLink",
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
                name: "IX_Plano_ImagemCapaId",
                table: "Plano",
                column: "ImagemCapaId");

            migrationBuilder.CreateIndex(
                name: "IX_Plano_ProprietarioId",
                table: "Plano",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItem_ImagemCapaId",
                table: "PlanoItem",
                column: "ImagemCapaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItem_PlanoId",
                table: "PlanoItem",
                column: "PlanoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcao_ImagemCapaId",
                table: "PlanoItemOpcao",
                column: "ImagemCapaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcao_PlanoItemId",
                table: "PlanoItemOpcao",
                column: "PlanoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcaoLink_PlanoItemOpcaoId",
                table: "PlanoItemOpcaoLink",
                column: "PlanoItemOpcaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcaoLinkImagem_ImagemId",
                table: "PlanoItemOpcaoLinkImagem",
                column: "ImagemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcaoLinkImagem_PlanoItemOpcaoLinkId_ImagemId",
                table: "PlanoItemOpcaoLinkImagem",
                columns: new[] { "PlanoItemOpcaoLinkId", "ImagemId" },
                unique: true,
                filter: "[DataDelecao] IS NULL");

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
                name: "PlanoItemOpcaoLinkImagem");

            migrationBuilder.DropTable(
                name: "UsuarioPapel");

            migrationBuilder.DropTable(
                name: "PlanoItemOpcaoLink");

            migrationBuilder.DropTable(
                name: "Papel");

            migrationBuilder.DropTable(
                name: "PlanoItemOpcao");

            migrationBuilder.DropTable(
                name: "PlanoItem");

            migrationBuilder.DropTable(
                name: "Plano");

            migrationBuilder.DropTable(
                name: "Imagem");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
