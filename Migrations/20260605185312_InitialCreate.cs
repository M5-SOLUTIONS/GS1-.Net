using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M5Storage.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_RECURSOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CATEGORIA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    QUANTIDADE = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    MINIMO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    CRITICO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    NIVEL = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    ULTIMA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RECURSOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_USUARIOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_USUARIOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_ALERTAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RECURSO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MENSAGEM = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    NIVEL = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    RESOLVIDO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_ALERTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ALERTAS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_ALERTAS_T_RECURSOS_RECURSO_ID",
                        column: x => x.RECURSO_ID,
                        principalTable: "T_RECURSOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_RECURSO_ENERGIA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_ENERGIA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RECURSO_ENERGIA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_RECURSO_ENERGIA_T_RECURSOS_ID",
                        column: x => x.ID,
                        principalTable: "T_RECURSOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_RECURSO_MEDICAMENTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VALIDADE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RECURSO_MEDICAMENTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_RECURSO_MEDICAMENTO_T_RECURSOS_ID",
                        column: x => x.ID,
                        principalTable: "T_RECURSOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MOVIMENTACOES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USUARIO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RECURSO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_MOVIMENTACAO = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    QUANTIDADE = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    DATA_MOVIMENTACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MOVIMENTACOES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MOVIMENTACOES_T_RECURSOS_RECURSO_ID",
                        column: x => x.RECURSO_ID,
                        principalTable: "T_RECURSOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MOVIMENTACOES_T_USUARIOS_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "T_USUARIOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_ALERTAS_RECURSO_ID",
                table: "T_ALERTAS",
                column: "RECURSO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MOVIMENTACOES_RECURSO_ID",
                table: "T_MOVIMENTACOES",
                column: "RECURSO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MOVIMENTACOES_USUARIO_ID",
                table: "T_MOVIMENTACOES",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_USUARIOS_EMAIL",
                table: "T_USUARIOS",
                column: "EMAIL",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ALERTAS");

            migrationBuilder.DropTable(
                name: "T_MOVIMENTACOES");

            migrationBuilder.DropTable(
                name: "T_RECURSO_ENERGIA");

            migrationBuilder.DropTable(
                name: "T_RECURSO_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "T_USUARIOS");

            migrationBuilder.DropTable(
                name: "T_RECURSOS");
        }
    }
}
