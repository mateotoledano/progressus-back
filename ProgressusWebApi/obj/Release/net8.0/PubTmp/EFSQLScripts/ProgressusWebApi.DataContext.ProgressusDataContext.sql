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
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [Ejercicios] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [ImagenMaquina] nvarchar(max) NULL,
        [VideoEjercicio] nvarchar(max) NULL,
        CONSTRAINT [PK_Ejercicios] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [GruposMusculares] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [ImagenGrupoMuscular] nvarchar(max) NULL,
        CONSTRAINT [PK_GruposMusculares] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [Membresias] (
        [Id] int NOT NULL IDENTITY,
        [MesesDuracion] int NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [Precio] int NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Membresias] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [ObjetivosDePlanes] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ObjetivosDePlanes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [Entrenadores] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NULL,
        [Apellido] nvarchar(max) NULL,
        [Telefono] nvarchar(max) NULL,
        [UserId] nvarchar(450) NULL,
        CONSTRAINT [PK_Entrenadores] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Entrenadores_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [Socios] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NULL,
        [Apellido] nvarchar(max) NULL,
        [Telefono] nvarchar(max) NULL,
        [UserId] nvarchar(450) NULL,
        CONSTRAINT [PK_Socios] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Socios_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [Musculos] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [GrupoMuscularId] int NOT NULL,
        [ImagenMusculo] nvarchar(max) NULL,
        CONSTRAINT [PK_Musculos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Musculos_GruposMusculares_GrupoMuscularId] FOREIGN KEY ([GrupoMuscularId]) REFERENCES [GruposMusculares] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [PlanesDeEntrenamiento] (
        [Id] int NOT NULL IDENTITY,
        [DueñoDePlanId] nvarchar(max) NOT NULL,
        [DueñoDelPlanId] nvarchar(450) NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [ObjetivoDelPlanId] int NOT NULL,
        [DiasPorSemana] int NOT NULL,
        [FechaDeCreacion] datetime2 NOT NULL,
        [EsPlantilla] bit NOT NULL,
        CONSTRAINT [PK_PlanesDeEntrenamiento] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PlanesDeEntrenamiento_AspNetUsers_DueñoDelPlanId] FOREIGN KEY ([DueñoDelPlanId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_PlanesDeEntrenamiento_ObjetivosDePlanes_ObjetivoDelPlanId] FOREIGN KEY ([ObjetivoDelPlanId]) REFERENCES [ObjetivosDePlanes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [MusculosDeEjercicios] (
        [EjercicioId] int NOT NULL,
        [MusculoId] int NOT NULL,
        CONSTRAINT [PK_MusculosDeEjercicios] PRIMARY KEY ([EjercicioId], [MusculoId]),
        CONSTRAINT [FK_MusculosDeEjercicios_Ejercicios_EjercicioId] FOREIGN KEY ([EjercicioId]) REFERENCES [Ejercicios] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MusculosDeEjercicios_Musculos_MusculoId] FOREIGN KEY ([MusculoId]) REFERENCES [Musculos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [AsignacionesDePlanes] (
        [SocioId] nvarchar(450) NOT NULL,
        [PlanDeEntrenamientoId] int NOT NULL,
        [fechaDeAsginacion] datetime2 NOT NULL,
        [esVigente] bit NOT NULL,
        CONSTRAINT [PK_AsignacionesDePlanes] PRIMARY KEY ([PlanDeEntrenamientoId], [SocioId]),
        CONSTRAINT [FK_AsignacionesDePlanes_AspNetUsers_SocioId] FOREIGN KEY ([SocioId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AsignacionesDePlanes_PlanesDeEntrenamiento_PlanDeEntrenamientoId] FOREIGN KEY ([PlanDeEntrenamientoId]) REFERENCES [PlanesDeEntrenamiento] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [DiasDePlan] (
        [Id] int NOT NULL IDENTITY,
        [PlanDeEntrenamientoId] int NOT NULL,
        [NumeroDeDia] int NOT NULL,
        CONSTRAINT [PK_DiasDePlan] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DiasDePlan_PlanesDeEntrenamiento_PlanDeEntrenamientoId] FOREIGN KEY ([PlanDeEntrenamientoId]) REFERENCES [PlanesDeEntrenamiento] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [EjerciciosDelDia] (
        [DiaDePlanId] int NOT NULL,
        [EjercicioId] int NOT NULL,
        [OrdenDeEjercicio] int NOT NULL,
        [Series] int NOT NULL,
        [Repeticiones] int NOT NULL,
        CONSTRAINT [PK_EjerciciosDelDia] PRIMARY KEY ([EjercicioId], [DiaDePlanId]),
        CONSTRAINT [FK_EjerciciosDelDia_DiasDePlan_DiaDePlanId] FOREIGN KEY ([DiaDePlanId]) REFERENCES [DiasDePlan] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EjerciciosDelDia_Ejercicios_EjercicioId] FOREIGN KEY ([EjercicioId]) REFERENCES [Ejercicios] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE TABLE [SeriesDeEjercicio] (
        [Id] int NOT NULL,
        [DiaDePlanId] int NOT NULL,
        [EjercicioId] int NOT NULL,
        [NumeroDeSerie] int NOT NULL,
        [SemanaDelPlan] int NOT NULL,
        [RepeticionesConcretadas] int NOT NULL,
        [fechaDeRealizacion] datetime2 NOT NULL,
        CONSTRAINT [PK_SeriesDeEjercicio] PRIMARY KEY ([Id], [DiaDePlanId], [EjercicioId]),
        CONSTRAINT [FK_SeriesDeEjercicio_EjerciciosDelDia_EjercicioId_DiaDePlanId] FOREIGN KEY ([EjercicioId], [DiaDePlanId]) REFERENCES [EjerciciosDelDia] ([EjercicioId], [DiaDePlanId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_AsignacionesDePlanes_SocioId] ON [AsignacionesDePlanes] ([SocioId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_DiasDePlan_PlanDeEntrenamientoId] ON [DiasDePlan] ([PlanDeEntrenamientoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_EjerciciosDelDia_DiaDePlanId] ON [EjerciciosDelDia] ([DiaDePlanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_Entrenadores_UserId] ON [Entrenadores] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_Musculos_GrupoMuscularId] ON [Musculos] ([GrupoMuscularId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_MusculosDeEjercicios_MusculoId] ON [MusculosDeEjercicios] ([MusculoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_PlanesDeEntrenamiento_DueñoDelPlanId] ON [PlanesDeEntrenamiento] ([DueñoDelPlanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_PlanesDeEntrenamiento_ObjetivoDelPlanId] ON [PlanesDeEntrenamiento] ([ObjetivoDelPlanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_SeriesDeEjercicio_EjercicioId_DiaDePlanId] ON [SeriesDeEjercicio] ([EjercicioId], [DiaDePlanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    CREATE INDEX [IX_Socios_UserId] ON [Socios] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027204205_Inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241027204205_Inicial', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE TABLE [EstadoSolicitudes] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_EstadoSolicitudes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE TABLE [TipoDePagos] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TipoDePagos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE TABLE [SolicitudDePagos] (
        [Id] int NOT NULL IDENTITY,
        [IdentityUserId] nvarchar(max) NOT NULL,
        [TipoDePagoId] int NOT NULL,
        [MembresiaId] int NOT NULL,
        CONSTRAINT [PK_SolicitudDePagos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SolicitudDePagos_Membresias_MembresiaId] FOREIGN KEY ([MembresiaId]) REFERENCES [Membresias] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SolicitudDePagos_TipoDePagos_TipoDePagoId] FOREIGN KEY ([TipoDePagoId]) REFERENCES [TipoDePagos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE TABLE [HistorialSolicitudDePagos] (
        [Id] int NOT NULL IDENTITY,
        [EstadoSolicitudId] int NOT NULL,
        [SolicitudDePagoId] int NOT NULL,
        [FechaCambioEstado] datetime2 NOT NULL,
        CONSTRAINT [PK_HistorialSolicitudDePagos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_HistorialSolicitudDePagos_EstadoSolicitudes_EstadoSolicitudId] FOREIGN KEY ([EstadoSolicitudId]) REFERENCES [EstadoSolicitudes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_HistorialSolicitudDePagos_SolicitudDePagos_SolicitudDePagoId] FOREIGN KEY ([SolicitudDePagoId]) REFERENCES [SolicitudDePagos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE INDEX [IX_HistorialSolicitudDePagos_EstadoSolicitudId] ON [HistorialSolicitudDePagos] ([EstadoSolicitudId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE INDEX [IX_HistorialSolicitudDePagos_SolicitudDePagoId] ON [HistorialSolicitudDePagos] ([SolicitudDePagoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE INDEX [IX_SolicitudDePagos_MembresiaId] ON [SolicitudDePagos] ([MembresiaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    CREATE INDEX [IX_SolicitudDePagos_TipoDePagoId] ON [SolicitudDePagos] ([TipoDePagoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241027230259_SolicitudDePago'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241027230259_SolicitudDePago', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241104234747_Membresiasss'
)
BEGIN
    ALTER TABLE [SolicitudDePagos] ADD [FechaCreacion] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241104234747_Membresiasss'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241104234747_Membresiasss', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241105210933_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241105210933_InitialCreate', N'8.0.7');
END;
GO

COMMIT;
GO

