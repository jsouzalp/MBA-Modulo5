Build started...
Build succeeded.
The Entity Framework tools version '9.0.0' is older than that of the runtime '9.0.8'. Update the tools for the latest features and bug fixes. See https://aka.ms/AAc1fbw for more information.
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Alunos] (
    [AlunoId] UniqueIdentifier NOT NULL,
    [CodigoUsuarioAutenticacao] UniqueIdentifier NOT NULL,
    [Nome] Varchar(100) NOT NULL,
    [Email] Varchar(100) NOT NULL,
    [Cpf] Varchar(11) NULL,
    [DataNascimento] SmallDateTime NOT NULL,
    [Telefone] Varchar(25) NULL,
    [Ativo] Bit NOT NULL,
    [Genero] Varchar(20) NOT NULL,
    [Cidade] Varchar(50) NOT NULL,
    [Estado] Varchar(2) NULL,
    [Cep] Varchar(8) NOT NULL,
    [Foto] Varchar(1024) NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [AlunosPK] PRIMARY KEY ([AlunoId])
);

CREATE TABLE [MatriculasCursos] (
    [MatriculaCursoId] UniqueIdentifier NOT NULL,
    [AlunoId] UniqueIdentifier NOT NULL,
    [CursoId] UniqueIdentifier NOT NULL,
    [NomeCurso] Varchar(200) NOT NULL,
    [Valor] Money NOT NULL,
    [DataMatricula] SmallDateTime NOT NULL,
    [DataConclusao] SmallDateTime NULL,
    [EstadoMatricula] TinyInt NOT NULL,
    [Observacao] Varchar(2000) NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [MatriculasCursosPK] PRIMARY KEY ([MatriculaCursoId]),
    CONSTRAINT [MatriculasCursosAlunosFK] FOREIGN KEY ([AlunoId]) REFERENCES [Alunos] ([AlunoId]) ON DELETE CASCADE
);

CREATE TABLE [Certificados] (
    [CertificadoId] UniqueIdentifier NOT NULL,
    [MatriculaCursoId] UniqueIdentifier NOT NULL,
    [NomeCurso] Varchar(200) NOT NULL,
    [DataSolicitacao] SmallDateTime NOT NULL,
    [DataEmissao] SmallDateTime NULL,
    [CargaHoraria] SmallInt NOT NULL,
    [NotaFinal] TinyInt NOT NULL,
    [PathCertificado] Varchar(1024) NOT NULL,
    [NomeInstrutor] Varchar(100) NOT NULL,
    [DataCriacao] DateTime NOT NULL,
    [DataAlteracao] DateTime NULL,
    CONSTRAINT [CertificadosPK] PRIMARY KEY ([CertificadoId]),
    CONSTRAINT [MatriculasCursosCertificadosFK] FOREIGN KEY ([MatriculaCursoId]) REFERENCES [MatriculasCursos] ([MatriculaCursoId]) ON DELETE CASCADE
);

CREATE TABLE [HistoricosAprendizado] (
    [HistoricoAprendizadoId] UniqueIdentifier NOT NULL,
    [MatriculaCursoId] UniqueIdentifier NOT NULL,
    [CursoId] UniqueIdentifier NOT NULL,
    [AulaId] UniqueIdentifier NOT NULL,
    [NomeAula] Varchar(100) NOT NULL,
    [CargaHoraria] Int NOT NULL,
    [DataInicio] SmallDateTime NOT NULL,
    [DataTermino] SmallDateTime NULL,
    CONSTRAINT [HistoricoAprendizadoPK] PRIMARY KEY ([HistoricoAprendizadoId]),
    CONSTRAINT [FK_HistoricosAprendizado_MatriculasCursos_MatriculaCursoId] FOREIGN KEY ([MatriculaCursoId]) REFERENCES [MatriculasCursos] ([MatriculaCursoId]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [AlunosEmailUK] ON [Alunos] ([Email]);

CREATE INDEX [AlunosNomeIDX] ON [Alunos] ([Nome]);

CREATE UNIQUE INDEX [CertificadosMatriculaCursoIdIDX] ON [Certificados] ([MatriculaCursoId]);

CREATE INDEX [HistoricosAprendizadoAulaIdIDX] ON [HistoricosAprendizado] ([AulaId]);

CREATE INDEX [HistoricosAprendizadoCursoIdIDX] ON [HistoricosAprendizado] ([CursoId]);

CREATE INDEX [IX_HistoricosAprendizado_MatriculaCursoId] ON [HistoricosAprendizado] ([MatriculaCursoId]);

CREATE INDEX [MatriculasCursosAlunoIdIDX] ON [MatriculasCursos] ([AlunoId]);

CREATE INDEX [MatriculasCursosCursoIdIDX] ON [MatriculasCursos] ([CursoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260104124631_InitialMigrationSqlServer', N'9.0.8');

COMMIT;
GO


