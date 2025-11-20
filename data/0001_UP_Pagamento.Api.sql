USE PagamentosDB
GO

IF ((SELECT COUNT(1) FROM SYS.TABLES WHERE NAME = 'Pagamentos') = 0)
BEGIN


BEGIN TRANSACTION;
CREATE TABLE [Pagamentos] (
    [Id] uniqueidentifier NOT NULL,
    [CobrancaCursoId] uniqueidentifier NOT NULL,
    [AlunoId] uniqueidentifier NOT NULL,
    [Status] varchar(100) NULL,
    [Valor] decimal(18,2) NOT NULL,
    [NomeCartao] varchar(250) NOT NULL,
    [NumeroCartao] varchar(16) NOT NULL,
    [ExpiracaoCartao] varchar(10) NOT NULL,
    [CvvCartao] varchar(4) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Pagamentos] PRIMARY KEY ([Id])
);

CREATE TABLE [Transacoes] (
    [Id] uniqueidentifier NOT NULL,
    [CobrancaCursoId] uniqueidentifier NOT NULL,
    [PagamentoId] uniqueidentifier NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [StatusTransacao] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Transacoes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transacoes_Pagamentos_PagamentoId] FOREIGN KEY ([PagamentoId]) REFERENCES [Pagamentos] ([Id])
);

CREATE UNIQUE INDEX [IX_Transacoes_PagamentoId] ON [Transacoes] ([PagamentoId]);

COMMIT;


END


