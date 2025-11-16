USE ConteudoDB
GO

IF ((SELECT COUNT(1) FROM SYS.TABLES WHERE NAME = 'Categoria') = 0)
BEGIN


BEGIN TRANSACTION;
CREATE TABLE [Categoria] (
    [CategoriaId] UniqueIdentifier NOT NULL,
    [Nome] Varchar(100) NOT NULL,
    [Descricao] Varchar(1024) NOT NULL,
    [Cor] Varchar(100) NOT NULL,
    [IconeUrl] Varchar(500) NULL,
    [IsAtiva] bit NOT NULL,
    [Ordem] int NOT NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [CategoriaPK] PRIMARY KEY ([CategoriaId])
);

CREATE TABLE [Cursos] (
    [CursoId] UniqueIdentifier NOT NULL,
    [Nome] Varchar(200) NOT NULL,
    [Valor] Money NOT NULL,
    [Ativo] bit NOT NULL,
    [ValidoAte] datetime2 NULL,
    [ConteudoProgramatico_Resumo] Varchar(250) NULL,
    [ConteudoProgramatico_Descricao] Varchar(500) NULL,
    [ConteudoProgramatico_Objetivos] Varchar(1024) NULL,
    [ConteudoProgramatico_PreRequisitos] Varchar(1024) NULL,
    [ConteudoProgramatico_PublicoAlvo] Varchar(1024) NULL,
    [ConteudoProgramatico_Metodologia] Varchar(1024) NULL,
    [ConteudoProgramatico_Recursos] Varchar(1024) NULL,
    [ConteudoProgramatico_Avaliacao] Varchar(1024) NULL,
    [ConteudoProgramatico_Bibliografia] Varchar(1024) NULL,
    [CategoriaId] UniqueIdentifier NULL,
    [DuracaoHoras] int NOT NULL,
    [Nivel] Varchar(50) NOT NULL,
    [ImagemUrl] Varchar(500) NULL,
    [Instrutor] Varchar(100) NOT NULL,
    [VagasMaximas] int NOT NULL,
    [VagasOcupadas] int NOT NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [CursosPK] PRIMARY KEY ([CursoId]),
    CONSTRAINT [FK_Cursos_Categoria_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categoria] ([CategoriaId]) ON DELETE SET NULL
);

CREATE TABLE [Aulas] (
    [AulaId] UniqueIdentifier NOT NULL,
    [CursoId] UniqueIdentifier NOT NULL,
    [Nome] Varchar(200) NOT NULL,
    [Descricao] Varchar(1024) NOT NULL,
    [Numero] int NOT NULL,
    [DuracaoMinutos] int NOT NULL,
    [VideoUrl] Varchar(500) NOT NULL,
    [TipoAula] Varchar(50) NOT NULL,
    [IsObrigatoria] bit NOT NULL,
    [IsPublicada] bit NOT NULL,
    [DataPublicacao] datetime2 NULL,
    [Observacoes] Varchar(1000) NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [AulasPK] PRIMARY KEY ([AulaId]),
    CONSTRAINT [FK_Aulas_Cursos_CursoId] FOREIGN KEY ([CursoId]) REFERENCES [Cursos] ([CursoId]) ON DELETE CASCADE
);

CREATE TABLE [Materiais] (
    [MaterialId] UniqueIdentifier NOT NULL,
    [AulaId] UniqueIdentifier NOT NULL,
    [Nome] Varchar(200) NOT NULL,
    [Descricao] Varchar(1024) NOT NULL,
    [TipoMaterial] Varchar(50) NOT NULL,
    [Url] Varchar(500) NOT NULL,
    [IsObrigatorio] bit NOT NULL,
    [TamanhoBytes] bigint NOT NULL,
    [Extensao] Varchar(10) NULL,
    [Ordem] int NOT NULL,
    [IsAtivo] bit NOT NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [MateriaisPK] PRIMARY KEY ([MaterialId]),
    CONSTRAINT [FK_Materiais_Aulas_AulaId] FOREIGN KEY ([AulaId]) REFERENCES [Aulas] ([AulaId]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [AlunosCursoIdNumeroIDX] ON [Aulas] ([CursoId], [Numero]);

CREATE INDEX [AlunosIsPublicadaIDX] ON [Aulas] ([IsPublicada]);

CREATE INDEX [AlunosNomeIDX] ON [Aulas] ([Nome]);

CREATE INDEX [AlunosTipoAulaIDX] ON [Aulas] ([TipoAula]);

CREATE INDEX [CategoriaIsAtivaIDX] ON [Categoria] ([IsAtiva]);

CREATE UNIQUE INDEX [CategoriaNomeIDX] ON [Categoria] ([Nome]);

CREATE INDEX [CategoriaOrdemIDX] ON [Categoria] ([Ordem]);

CREATE INDEX [CursoCategoriaIdIDX] ON [Cursos] ([CategoriaId]);

CREATE UNIQUE INDEX [CursoNomeIDX] ON [Cursos] ([Nome]);

CREATE INDEX [CursoValidoAteIDX] ON [Cursos] ([ValidoAte]);

CREATE UNIQUE INDEX [MaterialAulaIdNomeIDX] ON [Materiais] ([AulaId], [Nome]);

CREATE INDEX [MaterialIsAtivoIDX] ON [Materiais] ([IsAtivo]);

CREATE INDEX [MaterialOrdemIDX] ON [Materiais] ([Ordem]);

CREATE INDEX [MaterialTipoMaterialIDX] ON [Materiais] ([TipoMaterial]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251116111048_InitialMigrationSqlServer', N'9.0.8');




END

