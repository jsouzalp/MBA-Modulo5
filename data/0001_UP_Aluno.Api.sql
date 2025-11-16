USE ALUNODB
GO

IF ((SELECT COUNT(1) FROM SYS.TABLES WHERE NAME = 'Alunos') = 0)
BEGIN


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

COMMIT;
GO




END

