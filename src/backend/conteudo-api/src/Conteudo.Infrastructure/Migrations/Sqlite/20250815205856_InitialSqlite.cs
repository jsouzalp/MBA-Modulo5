using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Conteudo.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class InitialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CategoriaId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Nome = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "Varchar", maxLength: 0, nullable: false),
                    Cor = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false),
                    IconeUrl = table.Column<string>(type: "Varchar", maxLength: 500, nullable: true),
                    IsAtiva = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CategoriaPK", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Nome = table.Column<string>(type: "Varchar", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "Money", precision: 10, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValidoAte = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConteudoProgramatico_Resumo = table.Column<string>(type: "Varchar", maxLength: 250, nullable: true),
                    ConteudoProgramatico_Descricao = table.Column<string>(type: "Varchar", maxLength: 500, nullable: true),
                    ConteudoProgramatico_Objetivos = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    ConteudoProgramatico_PreRequisitos = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    ConteudoProgramatico_PublicoAlvo = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    ConteudoProgramatico_Metodologia = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    ConteudoProgramatico_Recursos = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    ConteudoProgramatico_Avaliacao = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    ConteudoProgramatico_Bibliografia = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: true),
                    CategoriaId = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    DuracaoHoras = table.Column<int>(type: "INTEGER", nullable: false),
                    Nivel = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false),
                    ImagemUrl = table.Column<string>(type: "Varchar", maxLength: 500, nullable: true),
                    Instrutor = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false),
                    VagasMaximas = table.Column<int>(type: "INTEGER", nullable: false),
                    VagasOcupadas = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CursosPK", x => x.CursoId);
                    table.ForeignKey(
                        name: "FK_Cursos_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Aulas",
                columns: table => new
                {
                    AulaId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CursoId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Nome = table.Column<string>(type: "Varchar", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "Varchar", maxLength: 1024, nullable: false),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    DuracaoMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    VideoUrl = table.Column<string>(type: "Varchar", maxLength: 500, nullable: false),
                    TipoAula = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false),
                    IsObrigatoria = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublicada = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "Varchar", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AulasPK", x => x.AulaId);
                    table.ForeignKey(
                        name: "FK_Aulas_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "CursoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materiais",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    AulaId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Nome = table.Column<string>(type: "Varchar", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "Varchar", maxLength: 0, nullable: false),
                    TipoMaterial = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "Varchar", maxLength: 500, nullable: false),
                    IsObrigatorio = table.Column<bool>(type: "INTEGER", nullable: false),
                    TamanhoBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    Extensao = table.Column<string>(type: "Varchar", maxLength: 10, nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAtivo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MateriaisPK", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_Materiais_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "AulaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "AlunosCursoIdNumeroIDX",
                table: "Aulas",
                columns: new[] { "CursoId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AlunosIsPublicadaIDX",
                table: "Aulas",
                column: "IsPublicada");

            migrationBuilder.CreateIndex(
                name: "AlunosNomeIDX",
                table: "Aulas",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "AlunosTipoAulaIDX",
                table: "Aulas",
                column: "TipoAula");

            migrationBuilder.CreateIndex(
                name: "CategoriaIsAtivaIDX",
                table: "Categoria",
                column: "IsAtiva");

            migrationBuilder.CreateIndex(
                name: "CategoriaNomeIDX",
                table: "Categoria",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CategoriaOrdemIDX",
                table: "Categoria",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "CursoCategoriaIdIDX",
                table: "Cursos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "CursoNomeIDX",
                table: "Cursos",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CursoValidoAteIDX",
                table: "Cursos",
                column: "ValidoAte");

            migrationBuilder.CreateIndex(
                name: "MaterialAulaIdNomeIDX",
                table: "Materiais",
                columns: new[] { "AulaId", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "MaterialIsAtivoIDX",
                table: "Materiais",
                column: "IsAtivo");

            migrationBuilder.CreateIndex(
                name: "MaterialOrdemIDX",
                table: "Materiais",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "MaterialTipoMaterialIDX",
                table: "Materiais",
                column: "TipoMaterial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materiais");

            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
