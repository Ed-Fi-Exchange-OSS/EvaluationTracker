-- SPDX-License-Identifier: Apache-2.0
-- Licensed to the Ed-Fi Alliance under one or more agreements.
-- The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
-- See the LICENSE and NOTICES files in the project root for more information.

CREATE TABLE eppeta.PerformanceEvaluation (
    EducationOrganizationId INT NOT NULL,
    EvaluationPeriodDescriptorId INT NOT NULL,
    PerformanceEvaluationTitle NVARCHAR(50) NOT NULL,
    PerformanceEvaluationTypeDescriptorId INT NOT NULL,
    SchoolYear SMALLINT NOT NULL,
    TermDescriptorId INT NOT NULL,
    PerformanceEvaluationDescription NVARCHAR(255) NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT PerformanceEvaluation_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.PerformanceEvaluation ADD CONSTRAINT [PerformanceEvaluation_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.PerformanceEvaluation ADD CONSTRAINT [PerformanceEvaluation_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO

CREATE TABLE eppeta.PerformanceEvaluationRating (
    EducationOrganizationId INT NOT NULL,
    EvaluationPeriodDescriptorId INT NOT NULL,
    PerformanceEvaluationTitle NVARCHAR(50) NOT NULL,
    PerformanceEvaluationTypeDescriptorId INT NOT NULL,
    SchoolYear SMALLINT NOT NULL,
    TermDescriptorId INT NOT NULL,
    ActualDate DATE NOT NULL,
    ActualDuration INT NULL,
    PerformanceEvaluationRatingLevelDescriptorId INT NULL,
    ActualTime TIME NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    UserId NVARCHAR(225) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT PerformanceEvaluationRating_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.PerformanceEvaluationRating ADD CONSTRAINT [PerformanceEvaluationRating_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.PerformanceEvaluationRating ADD CONSTRAINT [PerformanceEvaluationRating_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO
ALTER TABLE [eppeta].[PerformanceEvaluationRating] WITH CHECK ADD CONSTRAINT [FK_PerformanceEvaluationRating_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO

CREATE TABLE eppeta.Evaluation (
    EvaluationTitle NVARCHAR(50) NOT NULL,
    EvaluationDescription NVARCHAR(255) NULL,
    MinRating DECIMAL(6, 3) NULL,
    MaxRating DECIMAL(6, 3) NULL,
    EvaluationTypeDescriptorId INT NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT Evaluation_PK PRIMARY KEY CLUSTERED (Id ASC)
);
ALTER TABLE eppeta.Evaluation ADD CONSTRAINT [Evaluation_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
ALTER TABLE eppeta.Evaluation ADD CONSTRAINT [Evaluation_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]

CREATE TABLE eppeta.EvaluationRating (
    EvaluationTitle NVARCHAR(50) NOT NULL,
    EvaluationRatingLevelDescriptorId INT NULL,
    EvaluationRatingStatusDescriptorId INT NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    UserId NVARCHAR(225) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationRating_PK PRIMARY KEY CLUSTERED (Id ASC)
    );
GO
ALTER TABLE eppeta.EvaluationRating ADD CONSTRAINT [EvaluationRating_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.EvaluationRating ADD CONSTRAINT [EvaluationRating_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO
ALTER TABLE [eppeta].[EvaluationRating] WITH CHECK ADD CONSTRAINT [FK_EvaluationRating_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO


CREATE TABLE eppeta.EvaluationObjective (
    EvaluationObjectiveTitle NVARCHAR(50) NOT NULL,
    EvaluationObjectiveDescription NVARCHAR(255) NULL,
    SortOrder INT NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationObjective_PK PRIMARY KEY CLUSTERED (Id ASC)
    );
GO
ALTER TABLE eppeta.EvaluationObjective ADD CONSTRAINT [EvaluationObjective_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.EvaluationObjective ADD CONSTRAINT [EvaluationObjective_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO

CREATE TABLE eppeta.EvaluationObjectiveRating (
    EvaluationObjectiveTitle NVARCHAR(50) NOT NULL,
    ObjectiveRatingLevelDescriptorId INT NULL,
    Comments NVARCHAR(1024) NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    UserId NVARCHAR(225) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationObjectiveRating_PK PRIMARY KEY CLUSTERED (Id ASC)
    );
GO
ALTER TABLE eppeta.EvaluationObjectiveRating ADD CONSTRAINT [EvaluationObjectiveRating_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.EvaluationObjectiveRating ADD CONSTRAINT [EvaluationObjectiveRating_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO
ALTER TABLE [eppeta].[EvaluationObjectiveRating] WITH CHECK ADD CONSTRAINT [FK_EvaluationObjectiveRating_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO

CREATE TABLE eppeta.EvaluationElement (
    EvaluationElementTitle NVARCHAR(255) NOT NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationElement_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.EvaluationElement ADD CONSTRAINT [EvaluationElement_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.EvaluationElement ADD CONSTRAINT [EvaluationElement_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO

CREATE TABLE eppeta.EvaluationElementRating (
    EvaluationElementTitle NVARCHAR(255) NOT NULL,
    EvaluationElementRatingLevelDescriptorId INT NULL,
    Rating DECIMAL(6, 3) NOT NULL,
    CreateDate DATETIME NOT NULL,
    LastModifiedDate DATETIME NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    UserId NVARCHAR(225) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationElementRating_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.EvaluationElementRating ADD CONSTRAINT [EvaluationElementRating_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.EvaluationElementRating ADD CONSTRAINT [EvaluationElementRating_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO
ALTER TABLE [eppeta].[EvaluationElementRating] WITH CHECK ADD CONSTRAINT [FK_EvaluationElementRating_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO
