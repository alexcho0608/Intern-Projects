
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/12/2016 17:10:45
-- Generated from EDMX file: C:\Users\Alex\Desktop\ArticleSite\DAL\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Publisher];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Article_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article] DROP CONSTRAINT [FK_Article_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_Article_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article] DROP CONSTRAINT [FK_Article_User];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseHistory_Article]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseHistory] DROP CONSTRAINT [FK_PurchaseHistory_Article];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseHistory_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseHistory] DROP CONSTRAINT [FK_PurchaseHistory_User];
GO
IF OBJECT_ID(N'[dbo].[FK_User_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Role];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Article]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Article];
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[PurchaseHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseHistory];
GO
IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Articles'
CREATE TABLE [dbo].[Articles] (
    [Name] nvarchar(20)  NOT NULL,
    [Description] nvarchar(20)  NOT NULL,
    [CategoryId] int  NOT NULL,
    [AuthorId] int  NOT NULL,
    [ArticleText] varchar(max)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Tax] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'PurchaseHistories'
CREATE TABLE [dbo].[PurchaseHistories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [ArticleId] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Login] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [RoleId] int  NOT NULL,
    [AccountMoney] decimal(19,4)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [PK_Articles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseHistories'
ALTER TABLE [dbo].[PurchaseHistories]
ADD CONSTRAINT [PK_PurchaseHistories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryId] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [FK_Article_Category]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Article_Category'
CREATE INDEX [IX_FK_Article_Category]
ON [dbo].[Articles]
    ([CategoryId]);
GO

-- Creating foreign key on [AuthorId] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [FK_Article_User]
    FOREIGN KEY ([AuthorId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Article_User'
CREATE INDEX [IX_FK_Article_User]
ON [dbo].[Articles]
    ([AuthorId]);
GO

-- Creating foreign key on [ArticleId] in table 'PurchaseHistories'
ALTER TABLE [dbo].[PurchaseHistories]
ADD CONSTRAINT [FK_PurchaseHistory_Article]
    FOREIGN KEY ([ArticleId])
    REFERENCES [dbo].[Articles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseHistory_Article'
CREATE INDEX [IX_FK_PurchaseHistory_Article]
ON [dbo].[PurchaseHistories]
    ([ArticleId]);
GO

-- Creating foreign key on [ClientId] in table 'PurchaseHistories'
ALTER TABLE [dbo].[PurchaseHistories]
ADD CONSTRAINT [FK_PurchaseHistory_User]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseHistory_User'
CREATE INDEX [IX_FK_PurchaseHistory_User]
ON [dbo].[PurchaseHistories]
    ([ClientId]);
GO

-- Creating foreign key on [RoleId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_Role'
CREATE INDEX [IX_FK_User_Role]
ON [dbo].[Users]
    ([RoleId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------