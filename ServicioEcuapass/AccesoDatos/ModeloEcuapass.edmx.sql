
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/20/2016 15:30:00
-- Generated from EDMX file: C:\Users\calvarado\Documents\Proyectos\S3\Portal_S3\ServicioEcuapass\AccesoDatos\ModeloEcuapass.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ecuapass];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ECU_CONSULTAS_ESTADO_FACTURAS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ECU_CONSULTAS_ESTADO_FACTURAS];
GO
IF OBJECT_ID(N'[dbo].[ECU_LIQUIDACION_BANCOS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ECU_LIQUIDACION_BANCOS];
GO
IF OBJECT_ID(N'[dbo].[ECU_LIQUIDACION_ESTADO]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ECU_LIQUIDACION_ESTADO];
GO
IF OBJECT_ID(N'[dbo].[ECU_LIQUIDACION_LOGS_WCF]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ECU_LIQUIDACION_LOGS_WCF];
GO
IF OBJECT_ID(N'[dbo].[ECU_LIQUIDACION_PAGO_SENAE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ECU_LIQUIDACION_PAGO_SENAE];
GO
IF OBJECT_ID(N'[dbo].[ECU_LIQUIDACION_SENAE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ECU_LIQUIDACION_SENAE];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ECU_LIQUIDACION_SENAE'
CREATE TABLE [dbo].[ECU_LIQUIDACION_SENAE] (
    [TRAMITE] char(1)  NOT NULL,
    [ESTADO] char(1)  NOT NULL,
    [CODIGO_UNICO] bigint  NOT NULL,
    [NUMERO_FACTURA] varchar(20)  NOT NULL,
    [FECHA_LIQUIDACION] datetime  NOT NULL,
    [FECHA_VIGENCIA] datetime  NOT NULL,
    [RAZON_SOCIAL] varchar(200)  NOT NULL,
    [NUMERO_IDENTIFICACION] varchar(50)  NOT NULL,
    [SUBTOTAL_SINIVA] decimal(16,4)  NOT NULL,
    [IVA] decimal(16,4)  NOT NULL,
    [MONTO_TOTAL] decimal(16,4)  NOT NULL,
    [NUMERO_LIQUIDACION] varchar(30)  NOT NULL,
    [FECHA_REGISTRO] datetime  NOT NULL,
    [USUARIO_REGISTRO] varchar(20)  NOT NULL,
    [ESTADO_PROCESO] char(1)  NOT NULL,
    [FECHA_PROCESO] datetime  NULL,
    [USUARIO_PROCESO] varchar(20)  NULL,
    [TIPO] varchar(5)  NULL,
    [NUMERO_BOOKING] nvarchar(max)  NULL,
    [ROL] varchar(20)  NULL
);
GO

-- Creating table 'ECU_LIQUIDACION_PAGO_SENAE'
CREATE TABLE [dbo].[ECU_LIQUIDACION_PAGO_SENAE] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ESTADO] char(1)  NOT NULL,
    [CODIGO_FACTURA] bigint  NOT NULL,
    [MONTO_RECAUDADO] decimal(16,4)  NULL,
    [CANAL_RECAUDO] varchar(10)  NOT NULL,
    [BANCO_PAGO] varchar(200)  NOT NULL,
    [FECHA_RECAUDACION] datetime  NOT NULL,
    [CODIGO_PAGO] varchar(20)  NOT NULL,
    [CODIGO_ACEPTACION_PAGO] varchar(20)  NOT NULL,
    [FECHA_CONTABLE] datetime  NOT NULL,
    [NUMERO_LIQUIDACION] varchar(30)  NULL,
    [TIPO_TRANSACCION] char(1)  NULL,
    [MOTIVO_PAGO] char(2)  NULL,
    [FECHA_REGISTRO] datetime  NOT NULL,
    [USUARIO_REGISTRO] varchar(30)  NULL,
    [CODIGO_FACTURA_ANTICIPO] bigint  NULL,
    [INVOICE] varchar(10)  NULL,
    [CUSTOMER] varchar(10)  NULL,
    [BANCO_SYSPRO] varchar(10)  NULL,
    [SYSPRO] varchar(3)  NULL,
    [CODIGO_CLIENTE] varchar(20)  NULL,
    [SOCIEDAD_SAP] varchar(20)  NULL,
    [ESTADO_SAP] varchar(20)  NULL,
    [MES_REGISTRO] varchar(20)  NULL,
    [MONEDA] varchar(20)  NULL,
    [NUMERO_ANTICIPO_SAP] varchar(20)  NULL,
    [NUMERO_FACTURA_REFERENCIA] varchar(20)  NULL,
    [MENSAJE_SAP] varchar(300)  NULL,
    [FECHA_ESTADO_SAP] datetime  NULL
);
GO

-- Creating table 'ECU_CONSULTAS_ESTADO_FACTURAS'
CREATE TABLE [dbo].[ECU_CONSULTAS_ESTADO_FACTURAS] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [NUMERO_LIQUIDACION] nvarchar(30)  NOT NULL,
    [VALOR_FACTURA] decimal(18,2)  NOT NULL,
    [VALOR_PAGADO] decimal(18,2)  NOT NULL,
    [VALOR_PENDIENTE] decimal(18,2)  NOT NULL,
    [FECHA] datetime  NOT NULL,
    [USUARIO] nvarchar(20)  NOT NULL,
    [FUE_OK] bit  NOT NULL,
    [MENSAJE] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ECU_LIQUIDACION_BANCOS'
CREATE TABLE [dbo].[ECU_LIQUIDACION_BANCOS] (
    [BANCO_SYSPRO] varchar(10)  NOT NULL,
    [BANCO_SENAE] varchar(100)  NOT NULL,
    [BANCO_BANRED] varchar(10)  NULL,
    [SAP_GL_CODE] varchar(20)  NULL
);
GO

-- Creating table 'ECU_LIQUIDACION_ESTADO'
CREATE TABLE [dbo].[ECU_LIQUIDACION_ESTADO] (
    [CODIGO_UNICO] bigint  NOT NULL,
    [NUMERO_LIQUIDACION] varchar(30)  NOT NULL,
    [ESTADO] char(1)  NOT NULL,
    [USUARIO] varchar(20)  NOT NULL,
    [FECHA] datetime  NOT NULL,
    [ID] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'ECU_LIQUIDACION_LOGS_WCF'
CREATE TABLE [dbo].[ECU_LIQUIDACION_LOGS_WCF] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FECHA] datetime  NOT NULL,
    [MENSAJE] nvarchar(255)  NOT NULL,
    [DETALLE] nvarchar(max)  NOT NULL,
    [ES_SERVICIO_ADUANA] bit  NOT NULL,
    [USUARIO] nvarchar(20)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CODIGO_UNICO] in table 'ECU_LIQUIDACION_SENAE'
ALTER TABLE [dbo].[ECU_LIQUIDACION_SENAE]
ADD CONSTRAINT [PK_ECU_LIQUIDACION_SENAE]
    PRIMARY KEY CLUSTERED ([CODIGO_UNICO] ASC);
GO

-- Creating primary key on [ID] in table 'ECU_LIQUIDACION_PAGO_SENAE'
ALTER TABLE [dbo].[ECU_LIQUIDACION_PAGO_SENAE]
ADD CONSTRAINT [PK_ECU_LIQUIDACION_PAGO_SENAE]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ECU_CONSULTAS_ESTADO_FACTURAS'
ALTER TABLE [dbo].[ECU_CONSULTAS_ESTADO_FACTURAS]
ADD CONSTRAINT [PK_ECU_CONSULTAS_ESTADO_FACTURAS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [BANCO_SYSPRO] in table 'ECU_LIQUIDACION_BANCOS'
ALTER TABLE [dbo].[ECU_LIQUIDACION_BANCOS]
ADD CONSTRAINT [PK_ECU_LIQUIDACION_BANCOS]
    PRIMARY KEY CLUSTERED ([BANCO_SYSPRO] ASC);
GO

-- Creating primary key on [ID] in table 'ECU_LIQUIDACION_ESTADO'
ALTER TABLE [dbo].[ECU_LIQUIDACION_ESTADO]
ADD CONSTRAINT [PK_ECU_LIQUIDACION_ESTADO]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ECU_LIQUIDACION_LOGS_WCF'
ALTER TABLE [dbo].[ECU_LIQUIDACION_LOGS_WCF]
ADD CONSTRAINT [PK_ECU_LIQUIDACION_LOGS_WCF]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CODIGO_FACTURA] in table 'ECU_LIQUIDACION_PAGO_SENAE'
ALTER TABLE [dbo].[ECU_LIQUIDACION_PAGO_SENAE]
ADD CONSTRAINT [FK_ECU_LIQUIDACION_PAGO_SENAE_FK]
    FOREIGN KEY ([CODIGO_FACTURA])
    REFERENCES [dbo].[ECU_LIQUIDACION_SENAE]
        ([CODIGO_UNICO])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ECU_LIQUIDACION_PAGO_SENAE_FK'
CREATE INDEX [IX_FK_ECU_LIQUIDACION_PAGO_SENAE_FK]
ON [dbo].[ECU_LIQUIDACION_PAGO_SENAE]
    ([CODIGO_FACTURA]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------