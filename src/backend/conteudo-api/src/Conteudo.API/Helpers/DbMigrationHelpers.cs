using Conteudo.Domain.Entities;
using Conteudo.Domain.ValueObjects;
using Conteudo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.API.Helpers;

[ExcludeFromCodeCoverage]
public static class DbMigrationHelpers
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        EnsureSeedData(app).Wait();
    }

    public static async Task EnsureSeedData(WebApplication application)
    {
        var service = application.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(service);
    }

    private static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConteudoDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker"))
        {
            await EnsureSeedData(context);
        }
    }

    private static async Task EnsureSeedData(ConteudoDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Categorias.Any())
        {
            var categoriaProgramacao = new Categoria(
                nome: "Programação",
                descricao: "Cursos voltados para programação e desenvolvimento de software",
                cor: "#4CAF50",
                iconeUrl: "https://exemplo.com/icones/programacao.png",
                ordem: 1);
            categoriaProgramacao.DefinirId(Guid.Parse("b2cfb629-db43-41f2-8318-f3c52a648b39"));

            var categoriaDevOps = new Categoria(
                nome: "DevOps",
                descricao: "Cursos voltados para práticas DevOps, containerização e CI/CD",
                cor: "#2196F3",
                iconeUrl: "https://exemplo.com/icones/devops.png",
                ordem: 2);
            categoriaDevOps.DefinirId(Guid.Parse("e3f6a7b8-c9d0-4a1b-2c3d-5e6f7a8b9c0d"));

            context.Categorias.AddRange(categoriaProgramacao, categoriaDevOps);
            await context.SaveChangesAsync();

            var cursos = new List<Curso>();
            var catProgramacaoId = context.Categorias.First(c => c.Nome == "Programação").Id;
            var catDevOpsId = context.Categorias.First(c => c.Nome == "DevOps").Id;

            cursos = new()
            {
                new Curso(
                    nome: "C# Avançado",
                    valor: 499.90m,
                    conteudoProgramatico: new ConteudoProgramatico(
                        resumo: "Aprenda recursos avançados do C#",
                        descricao: "Delegates, eventos, LINQ, Async/Await",
                        objetivos: "Dominar recursos avançados do C#",
                        preRequisitos: "C# básico",
                        publicoAlvo: "Desenvolvedores iniciantes/intermediários",
                        metodologia: "Aulas gravadas e exercícios práticos",
                        recursos: "Vídeos, PDFs e exemplos de código",
                        avaliacao: "Projetos práticos",
                        bibliografia: "Documentação Microsoft"
                    ),
                    duracaoHoras: 50,
                    nivel: "Intermediário",
                    instrutor: "Maria Souza",
                    vagasMaximas: 20,
                    imagemUrl: "http://localhost:4200/assets/images/curso-csharp-avancado.jpg",
                    validoAte: DateTime.UtcNow.AddYears(1),
                    categoriaId: catProgramacaoId
                ),
                new Curso(
                    nome: "SQL Server do Zero ao Avançado",
                    valor: 399.90m,
                    conteudoProgramatico: new ConteudoProgramatico(
                        resumo: "Aprenda SQL Server na prática",
                        descricao: "Criação de bancos, queries, stored procedures, índices",
                        objetivos: "Dominar SQL Server para aplicações profissionais",
                        preRequisitos: "Noções de banco de dados",
                        publicoAlvo: "Estudantes e desenvolvedores",
                        metodologia: "Aulas gravadas e exercícios",
                        recursos: "Vídeos e scripts SQL",
                        avaliacao: "Exercícios e projetos",
                        bibliografia: "Documentação Microsoft"
                    ),
                    duracaoHoras: 45,
                    nivel: "Básico/Intermediário",
                    instrutor: "Carlos Lima",
                    vagasMaximas: 25,
                    imagemUrl: "http://localhost:4200/assets/images/curso-sql.jpg",
                    validoAte: DateTime.UtcNow.AddYears(1),
                    categoriaId: catProgramacaoId
                ),
                new Curso(
                    nome: "Docker para Desenvolvedores",
                    valor: 349.90m,
                    conteudoProgramatico: new ConteudoProgramatico(
                        resumo: "Containerize aplicações com Docker",
                        descricao: "Conceitos de containers, imagens, Docker Compose",
                        objetivos: "Habilitar deploy de aplicações em containers",
                        preRequisitos: "Conhecimento básico de programação",
                        publicoAlvo: "Desenvolvedores e DevOps iniciantes",
                        metodologia: "Exemplos práticos e exercícios",
                        recursos: "Vídeos e arquivos de configuração",
                        avaliacao: "Projetos práticos",
                        bibliografia: "Documentação oficial Docker"
                    ),
                    duracaoHoras: 30,
                    nivel: "Básico",
                    instrutor: "Ana Pereira",
                    vagasMaximas: 20,
                    imagemUrl: "http://localhost:4200/assets/images/curso-docker.jpg",
                    validoAte: DateTime.UtcNow.AddYears(1),
                    categoriaId: catDevOpsId
                ),
                new Curso(
                    nome: "Angular Essencial",
                    valor: 449.90m,
                    conteudoProgramatico: new ConteudoProgramatico(
                        resumo: "Aprenda a construir aplicações SPA com Angular",
                        descricao: "Componentes, serviços, rotas, forms e Material",
                        objetivos: "Desenvolver aplicações Angular do zero",
                        preRequisitos: "HTML, CSS e JS básico",
                        publicoAlvo: "Iniciantes em Angular",
                        metodologia: "Aulas gravadas e exercícios práticos",
                        recursos: "Vídeos, exemplos de código",
                        avaliacao: "Exercícios e projetos",
                        bibliografia: "Documentação Angular"
                    ),
                    duracaoHoras: 60,
                    nivel: "Básico/Intermediário",
                    instrutor: "Ricardo Martins",
                    vagasMaximas: 25,
                    imagemUrl: "http://localhost:4200/assets/images/curso-angular.jpg",
                    validoAte: DateTime.UtcNow.AddYears(1),
                    categoriaId: catProgramacaoId
                ),
                new Curso(
                    nome: "Python para Iniciantes",
                    valor: 299.90m,
                    conteudoProgramatico: new ConteudoProgramatico(
                        resumo: "Aprenda Python do zero",
                        descricao: "Sintaxe, tipos, estruturas de controle, funções e módulos",
                        objetivos: "Introduzir o aluno à programação com Python",
                        preRequisitos: "Nenhum",
                        publicoAlvo: "Iniciantes em programação",
                        metodologia: "Aulas gravadas e exercícios práticos",
                        recursos: "Vídeos, PDFs e exemplos de código",
                        avaliacao: "Exercícios e projeto final",
                        bibliografia: "Documentação Python"
                    ),
                    duracaoHoras: 40,
                    nivel: "Básico",
                    instrutor: "João Oliveira",
                    vagasMaximas: 30,
                    imagemUrl: "http://localhost:4200/assets/images/curso-python.jpg",
                    validoAte: DateTime.UtcNow.AddYears(1),
                    categoriaId: catProgramacaoId
                ),
                new Curso(
                nome: "Kubernetes para DevOps",
                valor: 499.90m,
                conteudoProgramatico: new ConteudoProgramatico(
                    resumo: "Orquestração de containers com Kubernetes",
                    descricao: "Pods, serviços, deployments, escalabilidade e monitoramento",
                    objetivos: "Preparar o aluno para administrar clusters Kubernetes",
                    preRequisitos: "Docker básico",
                    publicoAlvo: "DevOps e desenvolvedores",
                    metodologia: "Aulas práticas e estudos de caso",
                    recursos: "Vídeos, arquivos de configuração",
                    avaliacao: "Projetos práticos",
                    bibliografia: "Documentação Kubernetes"
                ),
                duracaoHoras: 55,
                nivel: "Avançado",
                instrutor: "Lucas Almeida",
                vagasMaximas: 20,
                imagemUrl: "http://localhost:4200/assets/images/curso-kubernetes.jpg",
                validoAte: DateTime.UtcNow.AddYears(1),
                categoriaId: catDevOpsId
                ),
                new Curso(
                nome: "JavaScript Moderno",
                valor: 399.90m,
                conteudoProgramatico: new ConteudoProgramatico(
                    resumo: "Domine o JavaScript moderno",
                    descricao: "ES6+, Promises, Async/Await, Fetch API",
                    objetivos: "Capacitar o aluno a desenvolver aplicações JS modernas",
                    preRequisitos: "HTML e CSS básicos",
                    publicoAlvo: "Desenvolvedores web iniciantes",
                    metodologia: "Aulas gravadas e exercícios práticos",
                    recursos: "Vídeos, exemplos de código",
                    avaliacao: "Exercícios e projeto final",
                    bibliografia: "Documentação MDN"
                ),
                duracaoHoras: 50,
                nivel: "Intermediário",
                instrutor: "Fernanda Costa",
                vagasMaximas: 25,
                imagemUrl: "http://localhost:4200/assets/images/curso-javascript.jpg",
                validoAte: DateTime.UtcNow.AddYears(1),
                categoriaId: catProgramacaoId
                ),
                new Curso(
                    nome: "Introdução ao Git e GitHub",
                valor: 199.90m,
                conteudoProgramatico: new ConteudoProgramatico(
                    resumo: "Controle de versão com Git e GitHub",
                    descricao: "Comandos Git, branches, pull requests e workflows",
                    objetivos: "Ensinar o uso do Git para controle de versão",
                    preRequisitos: "Nenhum",
                    publicoAlvo: "Desenvolvedores iniciantes",
                    metodologia: "Aulas gravadas e exercícios práticos",
                    recursos: "Vídeos e exemplos de repositórios",
                    avaliacao: "Exercícios práticos",
                    bibliografia: "Documentação oficial Git"
                ),
                duracaoHoras: 20,
                nivel: "Básico",
                instrutor: "Patrícia Fernandes",
                vagasMaximas: 30,
                imagemUrl: "http://localhost:4200/assets/images/curso-git.jpg",
                validoAte: DateTime.UtcNow.AddYears(1),
                categoriaId: catDevOpsId
                )
            };

            cursos[0].DefinirId(Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"));
            cursos[1].DefinirId(Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"));
            cursos[2].DefinirId(Guid.Parse("b1c2d3e4-f5a6-7b8c-9d0e-1f2a3b4c5d6e"));
            cursos[3].DefinirId(Guid.Parse("c1d2e3f4-a5b6-7c8d-9e0f-1a2b3c4d5e6f"));
            cursos[4].DefinirId(Guid.Parse("d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6a"));
            cursos[5].DefinirId(Guid.Parse("e1f2a3b4-c5d6-7e8f-9a0b-1c2d3e4f5a6b"));
            cursos[6].DefinirId(Guid.Parse("f2a3b4c5-d6e7-8f9a-0b1c-2d3e4f5a6b7c"));
            cursos[7].DefinirId(Guid.Parse("a2b3c4d5-e6f7-8a9b-0c1d-2e3f4a5b6c7d"));

            context.Cursos.AddRange(cursos);
            await context.SaveChangesAsync();

            var matriz = new Dictionary<Guid, List<Guid>>
            {
                { cursos[0].Id, [Guid.Parse("9be503ca-83fb-41cb-98e4-8f0ae98692a0"), Guid.Parse("c55fd2e3-9a07-4b1d-8b35-237c12712ad4"), Guid.Parse("fbe91473-7e59-414b-90ef-c9a13b3c24ec")] },
                { cursos[1].Id, [Guid.Parse("84d09a65-8ac1-4bde-83a4-8533ab3b97a4"), Guid.Parse("6557645c-5879-4120-a6ed-a5349a3701c8"), Guid.Parse("db77ee62-b666-47d4-8e5b-c651c81e7fac")] },
                { cursos[2].Id, [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()] },
                { cursos[3].Id, [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()] },
                { cursos[4].Id, [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()] },
                { cursos[5].Id, [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()] },
                { cursos[6].Id, [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()] },
                { cursos[7].Id, [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()] }
            };

            for (int z = 0; z < 8; z++)
            {
                var curso = cursos[z];

                for (int i = 1; i <= 3; i++)
                {
                    var aula = new Aula(
                        cursoId: curso.Id,
                        nome: $"Aula {i} - {curso.Nome}",
                        descricao: $"Conteúdo da aula {i} do curso {curso.Nome}",
                        numero: i,
                        duracaoMinutos: 40 + i * 5,
                        videoUrl: $"https://exemplo.com/videos/{curso.Nome.ToLower().Replace(" ", "-")}-aula{i}.mp4",
                        tipoAula: "Teórica",
                        isObrigatoria: true,
                        observacoes: "Assistir antes de prosseguir"
                    );

                    aula.DefinirId(matriz[cursos[z].Id][i - 1]);
                    context.Aulas.Add(aula);

                    var material = new Material(
                        aulaId: aula.Id,
                        nome: $"Material da Aula {i}",
                        descricao: $"Arquivo complementar da aula {i} do curso {curso.Nome}",
                        tipoMaterial: "PDF",
                        url: $"https://exemplo.com/materiais/{curso.Nome.ToLower().Replace(" ", "-")}-aula{i}.pdf",
                        isObrigatorio: true,
                        tamanhoBytes: 1024 * 300,
                        extensao: ".pdf",
                        ordem: i
                    );
                    material.DefinirId(Guid.NewGuid());
                    context.Materiais.Add(material);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
