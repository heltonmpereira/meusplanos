using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeusPlanos.Modelo.Migrations
{
    /// <inheritdoc />
    public partial class ImagensMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImagemCapaId",
                table: "PlanoItemOpcao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImagemCapaId",
                table: "PlanoItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImagemCapaId",
                table: "Plano",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItemOpcao_ImagemCapaId",
                table: "PlanoItemOpcao",
                column: "ImagemCapaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoItem_ImagemCapaId",
                table: "PlanoItem",
                column: "ImagemCapaId");

            migrationBuilder.CreateIndex(
                name: "IX_Plano_ImagemCapaId",
                table: "Plano",
                column: "ImagemCapaId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Plano_Imagem_ImagemCapaId",
                table: "Plano",
                column: "ImagemCapaId",
                principalTable: "Imagem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItem_Imagem_ImagemCapaId",
                table: "PlanoItem",
                column: "ImagemCapaId",
                principalTable: "Imagem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItemOpcao_Imagem_ImagemCapaId",
                table: "PlanoItemOpcao",
                column: "ImagemCapaId",
                principalTable: "Imagem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plano_Imagem_ImagemCapaId",
                table: "Plano");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItem_Imagem_ImagemCapaId",
                table: "PlanoItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItemOpcao_Imagem_ImagemCapaId",
                table: "PlanoItemOpcao");

            migrationBuilder.DropTable(
                name: "PlanoItemOpcaoLinkImagem");

            migrationBuilder.DropTable(
                name: "Imagem");

            migrationBuilder.DropIndex(
                name: "IX_PlanoItemOpcao_ImagemCapaId",
                table: "PlanoItemOpcao");

            migrationBuilder.DropIndex(
                name: "IX_PlanoItem_ImagemCapaId",
                table: "PlanoItem");

            migrationBuilder.DropIndex(
                name: "IX_Plano_ImagemCapaId",
                table: "Plano");

            migrationBuilder.DropColumn(
                name: "ImagemCapaId",
                table: "PlanoItemOpcao");

            migrationBuilder.DropColumn(
                name: "ImagemCapaId",
                table: "PlanoItem");

            migrationBuilder.DropColumn(
                name: "ImagemCapaId",
                table: "Plano");
        }
    }
}
