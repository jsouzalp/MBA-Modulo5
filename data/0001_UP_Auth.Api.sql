USE AuthDB
GO

IF ((SELECT COUNT(1) FROM SYS.TABLES WHERE NAME = 'AspNetRoles') = 0)
BEGIN
  BEGIN TRANSACTION;
  CREATE TABLE [AspNetRoles] (
      [Id] nvarchar(450) NOT NULL,
      [Name] nvarchar(256) NULL,
      [NormalizedName] nvarchar(256) NULL,
      [ConcurrencyStamp] nvarchar(max) NULL,
      CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
  );
  
  CREATE TABLE [AspNetUsers] (
      [Id] nvarchar(450) NOT NULL,
      [Nome] nvarchar(100) NOT NULL,
      [DataNascimento] datetime2 NOT NULL,
      [CPF] nvarchar(14) NOT NULL,
      [Telefone] nvarchar(20) NULL,
      [Genero] nvarchar(20) NULL,
      [Cidade] nvarchar(100) NULL,
      [Estado] nvarchar(50) NULL,
      [CEP] nvarchar(10) NULL,
      [Foto] nvarchar(500) NULL,
      [DataCadastro] datetime2 NOT NULL,
      [Ativo] bit NOT NULL,
      [RefreshToken] uniqueidentifier NULL,
      [RefreshTokenExpiryTime] datetime2 NULL,
      [UserName] nvarchar(256) NULL,
      [NormalizedUserName] nvarchar(256) NULL,
      [Email] nvarchar(256) NULL,
      [NormalizedEmail] nvarchar(256) NULL,
      [EmailConfirmed] bit NOT NULL,
      [PasswordHash] nvarchar(max) NULL,
      [SecurityStamp] nvarchar(max) NULL,
      [ConcurrencyStamp] nvarchar(max) NULL,
      [PhoneNumber] nvarchar(max) NULL,
      [PhoneNumberConfirmed] bit NOT NULL,
      [TwoFactorEnabled] bit NOT NULL,
      [LockoutEnd] datetimeoffset NULL,
      [LockoutEnabled] bit NOT NULL,
      [AccessFailedCount] int NOT NULL,
      CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
  );
  
  CREATE TABLE [RefreshTokens] (
      [Id] uniqueidentifier NOT NULL,
      [Username] nvarchar(max) NULL,
      [Token] uniqueidentifier NOT NULL,
      [ExpirationDate] datetime2 NOT NULL,
      CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id])
  );
  
  CREATE TABLE [SecurityKeys] (
      [Id] uniqueidentifier NOT NULL,
      [KeyId] nvarchar(max) NULL,
      [Type] nvarchar(max) NULL,
      [Use] nvarchar(max) NULL,
      [Parameters] nvarchar(max) NULL,
      [IsRevoked] bit NOT NULL,
      [RevokedReason] nvarchar(max) NULL,
      [CreationDate] datetime2 NOT NULL,
      [ExpiredAt] datetime2 NULL,
      CONSTRAINT [PK_SecurityKeys] PRIMARY KEY ([Id])
  );
  
  CREATE TABLE [AspNetRoleClaims] (
      [Id] int NOT NULL IDENTITY,
      [RoleId] nvarchar(450) NOT NULL,
      [ClaimType] nvarchar(max) NULL,
      [ClaimValue] nvarchar(max) NULL,
      CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
      CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
  );
  
  CREATE TABLE [AspNetUserClaims] (
      [Id] int NOT NULL IDENTITY,
      [UserId] nvarchar(450) NOT NULL,
      [ClaimType] nvarchar(max) NULL,
      [ClaimValue] nvarchar(max) NULL,
      CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
      CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
  );
  
  CREATE TABLE [AspNetUserLogins] (
      [LoginProvider] nvarchar(450) NOT NULL,
      [ProviderKey] nvarchar(450) NOT NULL,
      [ProviderDisplayName] nvarchar(max) NULL,
      [UserId] nvarchar(450) NOT NULL,
      CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
      CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
  );
  
  CREATE TABLE [AspNetUserRoles] (
      [UserId] nvarchar(450) NOT NULL,
      [RoleId] nvarchar(450) NOT NULL,
      CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
      CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
      CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
  );
  
  CREATE TABLE [AspNetUserTokens] (
      [UserId] nvarchar(450) NOT NULL,
      [LoginProvider] nvarchar(450) NOT NULL,
      [Name] nvarchar(450) NOT NULL,
      [Value] nvarchar(max) NULL,
      CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
      CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
  );
  
  CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
  
  CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
  
  CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
  
  CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
  
  CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
  
  CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
  
  CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
  
  /*
  delete from AspNetUserRoles
  delete from AspNetUserClaims
  delete from RefreshTokens
  delete from AspNetRoleClaims
  delete from AspNetUserLogins
  delete from AspNetUserTokens
  delete from SecurityKeys
  delete from AspNetUsers
  delete from AspNetRoles
  */
  
  /* Criação das ROLES */
  Declare @roleAdmin varchar(36) = '0837209a-cd1e-43bb-bf9f-be3c9916b634'
  Declare @roleAluno varchar(36) = '64226363-a39f-4287-a903-4d250feaec5f'
  Insert Into AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) Values (@roleAdmin, 'Administrador', 'Administrador', GetDate())
  Insert Into AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) Values (@roleAluno, 'Usuario', 'Usuario', GetDate())
  
  /* Criação de usuários templates - Senha padrão: Senh@Padr@o@123 */
  Declare @usuarioId varchar(36)
  Declare @nome varchar(50)
  Declare @email varchar(50)
  Declare @password varchar(100) = 'AQAAAAIAAYagAAAAEAk25ZhV81u6lXmfu9qoFm/8ZnOIC/sca9J8MnG3T1ISM7KdMVV03rbmwdmcDMl2cg=='
  
  -- User: admin@auth.api / pass: Senh@Padr@o@123
  Set @usuarioId = '180bf09a-e6db-41f7-9451-c8bc97a48731'
  Set @nome = 'Administrador'
  Set @email = 'admin@auth.api'
  Insert Into AspNetUsers (Id, Nome, DataNascimento, CPF, Telefone, Genero, Cidade, Estado, CEP, Foto, DataCadastro, Ativo, RefreshToken, RefreshTokenExpiryTime, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
  Values (@usuarioId, @nome, '20000101', '00000000000', '00000000000', 'M', 'RJ', 'RJ', '00000000', NULL, GetDate(), 1, NULL, NULL, @email, @email, @email, @email, 1, 'AQAAAAIAAYagAAAAEAk25ZhV81u6lXmfu9qoFm/8ZnOIC/sca9J8MnG3T1ISM7KdMVV03rbmwdmcDMl2cg==', 'F7UB22NUMPRAAMO2GODIS53MUWLKMTFG', '88651daf-60f6-40a5-aed9-b083e380520b', NULL, 0, 0, NULL, 1, 1)
  Insert Into AspNetUserRoles (UserId, RoleId) Values (@usuarioId, @roleAdmin)
  
  -- User: aluno1@auth.api / pass: Senh@Padr@o@123
  Set @usuarioId = 'aca5fe2a-988d-4bf3-8c5d-985b5d0aa038'
  Set @nome = 'Aluno 1'
  Set @email = 'aluno1@auth.api'
  Insert Into AspNetUsers (Id, Nome, DataNascimento, CPF, Telefone, Genero, Cidade, Estado, CEP, Foto, DataCadastro, Ativo, RefreshToken, RefreshTokenExpiryTime, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
  Values (@usuarioId, @nome, '20000101', '00000000000', '00000000000', 'M', 'RJ', 'RJ', '00000000', NULL, GetDate(), 1, NULL, NULL, @email, @email, @email, @email, 1, 'AQAAAAIAAYagAAAAEAk25ZhV81u6lXmfu9qoFm/8ZnOIC/sca9J8MnG3T1ISM7KdMVV03rbmwdmcDMl2cg==', 'F7UB22NUMPRAAMO2GODIS53MUWLKMTFG', '88651daf-60f6-40a5-aed9-b083e380520b', NULL, 0, 0, NULL, 1, 1)
  Insert Into AspNetUserRoles (UserId, RoleId) Values (@usuarioId, @roleAdmin)
  
  -- User: aluno2@auth.api / pass: Senh@Padr@o@123
  Set @usuarioId = 'a1a3abbf-10a9-49c9-90b2-0dcacb2bb44e'
  Set @nome = 'Aluno 2'
  Set @email = 'aluno2@auth.api'
  Insert Into AspNetUsers (Id, Nome, DataNascimento, CPF, Telefone, Genero, Cidade, Estado, CEP, Foto, DataCadastro, Ativo, RefreshToken, RefreshTokenExpiryTime, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
  Values (@usuarioId, @nome, '20000101', '00000000000', '00000000000', 'M', 'RJ', 'RJ', '00000000', NULL, GetDate(), 1, NULL, NULL, @email, @email, @email, @email, 1, 'AQAAAAIAAYagAAAAEAk25ZhV81u6lXmfu9qoFm/8ZnOIC/sca9J8MnG3T1ISM7KdMVV03rbmwdmcDMl2cg==', 'F7UB22NUMPRAAMO2GODIS53MUWLKMTFG', '88651daf-60f6-40a5-aed9-b083e380520b', NULL, 0, 0, NULL, 1, 1)
  Insert Into AspNetUserRoles (UserId, RoleId) Values (@usuarioId, @roleAdmin)
  
  COMMIT;
END

