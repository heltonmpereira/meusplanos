using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeusPlanos.Modelo.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDelecaoPlanoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItem_Plano_PlanoId",
                table: "PlanoItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItemOpcao_PlanoItem_PlanoItemId",
                table: "PlanoItemOpcao");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItemOpcaoLink_PlanoItemOpcao_PlanoItemOpcaoId",
                table: "PlanoItemOpcaoLink");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItem_Plano_PlanoId",
                table: "PlanoItem",
                column: "PlanoId",
                principalTable: "Plano",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItemOpcao_PlanoItem_PlanoItemId",
                table: "PlanoItemOpcao",
                column: "PlanoItemId",
                principalTable: "PlanoItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItemOpcaoLink_PlanoItemOpcao_PlanoItemOpcaoId",
                table: "PlanoItemOpcaoLink",
                column: "PlanoItemOpcaoId",
                principalTable: "PlanoItemOpcao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItem_Plano_PlanoId",
                table: "PlanoItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItemOpcao_PlanoItem_PlanoItemId",
                table: "PlanoItemOpcao");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoItemOpcaoLink_PlanoItemOpcao_PlanoItemOpcaoId",
                table: "PlanoItemOpcaoLink");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItem_Plano_PlanoId",
                table: "PlanoItem",
                column: "PlanoId",
                principalTable: "Plano",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItemOpcao_PlanoItem_PlanoItemId",
                table: "PlanoItemOpcao",
                column: "PlanoItemId",
                principalTable: "PlanoItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoItemOpcaoLink_PlanoItemOpcao_PlanoItemOpcaoId",
                table: "PlanoItemOpcaoLink",
                column: "PlanoItemOpcaoId",
                principalTable: "PlanoItemOpcao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
