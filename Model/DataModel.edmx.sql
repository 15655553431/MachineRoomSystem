
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/15/2019 11:20:02
-- Generated from EDMX file: F:\XLMachine\Model\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [XLMachine];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MachineroomComputerinfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Computerinfo] DROP CONSTRAINT [FK_MachineroomComputerinfo];
GO
IF OBJECT_ID(N'[dbo].[FK_ComputerinfoReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [FK_ComputerinfoReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_UserinfoNewsinfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Newsinfo] DROP CONSTRAINT [FK_UserinfoNewsinfo];
GO
IF OBJECT_ID(N'[dbo].[FK_UserinfoMachineroom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Machineroom] DROP CONSTRAINT [FK_UserinfoMachineroom];
GO
IF OBJECT_ID(N'[dbo].[FK_UserinfoReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [FK_UserinfoReservation];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Userinfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Userinfo];
GO
IF OBJECT_ID(N'[dbo].[Newsinfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Newsinfo];
GO
IF OBJECT_ID(N'[dbo].[Machineroom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Machineroom];
GO
IF OBJECT_ID(N'[dbo].[Computerinfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Computerinfo];
GO
IF OBJECT_ID(N'[dbo].[Reservation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservation];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Userinfo'
CREATE TABLE [dbo].[Userinfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(32)  NOT NULL,
    [Uname] nvarchar(32)  NOT NULL,
    [Pwd] nvarchar(32)  NOT NULL,
    [Logindate] datetime  NULL,
    [Signdate] datetime  NULL,
    [Number] nvarchar(32)  NULL,
    [Phone] nvarchar(16)  NULL,
    [Address] nvarchar(200)  NULL,
    [Status] int  NULL,
    [Type] int  NULL,
    [Other] nvarchar(500)  NULL,
    [Delflag] int  NULL
);
GO

-- Creating table 'Newsinfo'
CREATE TABLE [dbo].[Newsinfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Inputdate] datetime  NULL,
    [Showtime] datetime  NOT NULL,
    [Endtime] datetime  NULL,
    [Type] int  NULL,
    [Title] nvarchar(100)  NOT NULL,
    [Content] nvarchar(1000)  NOT NULL,
    [Other] nvarchar(200)  NULL,
    [Delflag] int  NULL,
    [UserinfoID] int  NOT NULL
);
GO

-- Creating table 'Machineroom'
CREATE TABLE [dbo].[Machineroom] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Number] nvarchar(16)  NOT NULL,
    [Address] nvarchar(200)  NULL,
    [Name] nvarchar(100)  NULL,
    [Count] int  NOT NULL,
    [Area] int  NOT NULL,
    [Crow] int  NOT NULL,
    [Ccol] int  NOT NULL,
    [IPinfo] nvarchar(100)  NULL,
    [Other] nvarchar(100)  NULL,
    [Status] int  NULL,
    [Delflag] int  NULL,
    [UserinfoID] int  NOT NULL
);
GO

-- Creating table 'Computerinfo'
CREATE TABLE [dbo].[Computerinfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Number] nvarchar(20)  NULL,
    [Row] int  NOT NULL,
    [Col] int  NOT NULL,
    [RN] nvarchar(32)  NULL,
    [Brand] nvarchar(50)  NULL,
    [Model] nvarchar(50)  NULL,
    [Facturedate] datetime  NULL,
    [MAC] nvarchar(50)  NULL,
    [IP] nvarchar(50)  NULL,
    [CPU] nvarchar(64)  NULL,
    [Memory] nvarchar(64)  NULL,
    [Hard] nvarchar(64)  NULL,
    [System] nvarchar(64)  NULL,
    [Status] int  NULL,
    [Money] decimal(18,0)  NULL,
    [Other] nvarchar(200)  NULL,
    [Delflag] int  NULL,
    [MachineroomID] int  NOT NULL
);
GO

-- Creating table 'Reservation'
CREATE TABLE [dbo].[Reservation] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Rdate] datetime  NOT NULL,
    [Sdate] datetime  NOT NULL,
    [DateFlag] int  NOT NULL,
    [Edate] datetime  NOT NULL,
    [Useinfo] nvarchar(500)  NULL,
    [Money] decimal(18,0)  NULL,
    [Status] int  NOT NULL,
    [Other] nvarchar(200)  NULL,
    [Delflag] int  NULL,
    [ComputerinfoID] int  NOT NULL,
    [UserinfoID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Userinfo'
ALTER TABLE [dbo].[Userinfo]
ADD CONSTRAINT [PK_Userinfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Newsinfo'
ALTER TABLE [dbo].[Newsinfo]
ADD CONSTRAINT [PK_Newsinfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Machineroom'
ALTER TABLE [dbo].[Machineroom]
ADD CONSTRAINT [PK_Machineroom]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Computerinfo'
ALTER TABLE [dbo].[Computerinfo]
ADD CONSTRAINT [PK_Computerinfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [PK_Reservation]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MachineroomID] in table 'Computerinfo'
ALTER TABLE [dbo].[Computerinfo]
ADD CONSTRAINT [FK_MachineroomComputerinfo]
    FOREIGN KEY ([MachineroomID])
    REFERENCES [dbo].[Machineroom]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MachineroomComputerinfo'
CREATE INDEX [IX_FK_MachineroomComputerinfo]
ON [dbo].[Computerinfo]
    ([MachineroomID]);
GO

-- Creating foreign key on [ComputerinfoID] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [FK_ComputerinfoReservation]
    FOREIGN KEY ([ComputerinfoID])
    REFERENCES [dbo].[Computerinfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ComputerinfoReservation'
CREATE INDEX [IX_FK_ComputerinfoReservation]
ON [dbo].[Reservation]
    ([ComputerinfoID]);
GO

-- Creating foreign key on [UserinfoID] in table 'Newsinfo'
ALTER TABLE [dbo].[Newsinfo]
ADD CONSTRAINT [FK_UserinfoNewsinfo]
    FOREIGN KEY ([UserinfoID])
    REFERENCES [dbo].[Userinfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserinfoNewsinfo'
CREATE INDEX [IX_FK_UserinfoNewsinfo]
ON [dbo].[Newsinfo]
    ([UserinfoID]);
GO

-- Creating foreign key on [UserinfoID] in table 'Machineroom'
ALTER TABLE [dbo].[Machineroom]
ADD CONSTRAINT [FK_UserinfoMachineroom]
    FOREIGN KEY ([UserinfoID])
    REFERENCES [dbo].[Userinfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserinfoMachineroom'
CREATE INDEX [IX_FK_UserinfoMachineroom]
ON [dbo].[Machineroom]
    ([UserinfoID]);
GO

-- Creating foreign key on [UserinfoID] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [FK_UserinfoReservation]
    FOREIGN KEY ([UserinfoID])
    REFERENCES [dbo].[Userinfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserinfoReservation'
CREATE INDEX [IX_FK_UserinfoReservation]
ON [dbo].[Reservation]
    ([UserinfoID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------