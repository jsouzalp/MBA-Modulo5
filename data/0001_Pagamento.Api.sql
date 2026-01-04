Build started...
Build succeeded.
The Entity Framework tools version '9.0.0' is older than that of the runtime '9.0.8'. Update the tools for the latest features and bug fixes. See https://aka.ms/AAc1fbw for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No store type was specified for the decimal property 'Valor' on entity type 'Pagamento'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType', specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion'.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No store type was specified for the decimal property 'Total' on entity type 'Transacao'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType', specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion'.
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

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260104124716_InitialMigrationSqlServer', N'9.0.8');

COMMIT;
GO


