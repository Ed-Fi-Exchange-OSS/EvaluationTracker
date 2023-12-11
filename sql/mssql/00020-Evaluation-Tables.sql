-- SPDX-License-Identifier: Apache-2.0
-- Licensed to the Ed-Fi Alliance under one or more agreements.
-- The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
-- See the LICENSE and NOTICES files in the project root for more information.

CREATE TABLE eppeta.PerformanceEvaluation (
    [EducationOrganizationId] [bigint] NOT NULL,
    [EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
    PerformanceEvaluationTitle NVARCHAR(50) NOT NULL,
    [PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
    SchoolYear SMALLINT NOT NULL,
    [TermDescriptor] [nvarchar](306) NOT NULL,
    PerformanceEvaluationDescription NVARCHAR(255) NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
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
    [EducationOrganizationId] [bigint] NOT NULL,
    EvaluationPeriodDescriptor [nvarchar](306) NOT NULL,
    PerformanceEvaluationTitle NVARCHAR(50) NOT NULL,
    PerformanceEvaluationTypeDescriptor [nvarchar](306) NOT NULL,
    [PersonId] [nvarchar](32) NOT NULL,
    [SourceSystemDescriptor] [nvarchar](306) NOT NULL,
    [ReviewedCandidateName] [nvarchar](255) NULL,
    [EvaluatorName] [nvarchar](255) NULL,
    [SchoolYear] [smallint] NOT NULL,
    TermDescriptor [nvarchar](306) NOT NULL,
    PerformanceEvaluationRatingLevelDescriptor [nvarchar](306) NULL,
    StartTime [datetime2](3) NOT NULL,
    EndTime [datetime2](3) NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    UserId NVARCHAR(225) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    [StatusId] INT NULL,
    CONSTRAINT PerformanceEvaluationRating_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.PerformanceEvaluationRating ADD CONSTRAINT [PerformanceEvaluationRating_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.PerformanceEvaluationRating ADD CONSTRAINT [PerformanceEvaluationRating_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO
ALTER TABLE eppeta.PerformanceEvaluationRating ADD CONSTRAINT [PerformanceEvaluationRating_DF_StartTime] DEFAULT (getdate()) FOR [StartTime]
GO
ALTER TABLE [eppeta].[PerformanceEvaluationRating] WITH CHECK ADD CONSTRAINT [FK_PerformanceEvaluationRating_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO
ALTER TABLE [eppeta].[PerformanceEvaluationRating] ADD CONSTRAINT [PerformanceEvaluationRating_DF_StatusId] DEFAULT (1) FOR [StatusId]
GO
ALTER TABLE [eppeta].[PerformanceEvaluationRating] WITH CHECK ADD CONSTRAINT [FK_PerformanceEvaluationRating_Status_StatusId] FOREIGN KEY([StatusId])
REFERENCES [eppeta].[Status] ([Id])
GO

CREATE TABLE eppeta.PerformanceEvaluationRatingLevel (
    [PerformanceEvaluationId] INT NULL,
    EvaluationRatingLevelDescriptor [nvarchar](306) NOT NULL,
    MaxRating [decimal](6,3) NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT PerformanceEvaluationRatingLevel_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE [eppeta].PerformanceEvaluationRatingLevel WITH CHECK ADD CONSTRAINT [FK_PerformanceEvaluationRatingLevel_PerformanceEvaluation_Id] FOREIGN KEY([PerformanceEvaluationId])
REFERENCES [eppeta].[PerformanceEvaluation] ([Id])
GO

CREATE TABLE eppeta.Evaluation (
    [EducationOrganizationId] [bigint] NOT NULL,
    EvaluationPeriodDescriptor [nvarchar](306) NOT NULL,
    EvaluationTitle NVARCHAR(50) NOT NULL,
    PerformanceEvaluationTitle NVARCHAR(50) NOT NULL,
    PerformanceEvaluationTypeDescriptor [nvarchar](306) NOT NULL,
    [SchoolYear] [smallint] NOT NULL,
    TermDescriptor [nvarchar](306) NOT NULL,
    EvaluationTypeDescriptor NVARCHAR(306) NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
    EdFi_Id NVARCHAR(50) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT Evaluation_PK PRIMARY KEY CLUSTERED (Id ASC)
);
ALTER TABLE eppeta.Evaluation ADD CONSTRAINT [Evaluation_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
ALTER TABLE eppeta.Evaluation ADD CONSTRAINT [Evaluation_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]


CREATE TABLE eppeta.EvaluationRating (
    [EducationOrganizationId] [bigint] NOT NULL,
    [EvaluationDate] [datetime2](3) NOT NULL,
    [EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
    [PersonId] [nvarchar](32) NOT NULL,
    [SchoolYear] [smallint] NOT NULL,
    [SourceSystemDescriptor] [nvarchar](306) NOT NULL,
    [TermDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationRatingLevelDescriptor] [nvarchar](306) NULL,
    [CandidateName] [nvarchar](50) NULL,
    [EvaluationRatingStatusDescriptor] [nvarchar](306) NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
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



CREATE TABLE [eppeta].[EvaluationObjective](
    [EducationOrganizationId] [bigint] NOT NULL,
    [EvaluationObjectiveTitle] [nvarchar](50) NOT NULL,
    [EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
    [SchoolYear] [smallint] NOT NULL,
    [TermDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationObjectiveDescription] [nvarchar](255) NULL,
    [EvaluationTypeDescriptor] [nvarchar](306) NULL,
    [SortOrder] [int] NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
    [EdFi_Id] [nvarchar](50) NULL,
    [Id] [int] IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationObjective_PK PRIMARY KEY CLUSTERED (Id ASC)
    );
GO
ALTER TABLE eppeta.EvaluationObjective ADD CONSTRAINT [EvaluationObjective_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.EvaluationObjective ADD CONSTRAINT [EvaluationObjective_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO

CREATE TABLE eppeta.EvaluationObjectiveRating (
    EvaluationObjectiveTitle NVARCHAR(50) NOT NULL,
    [EducationOrganizationId] [bigint] NOT NULL,
    [EvaluationDate] [datetime2](3) NOT NULL,
    [EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
    [PersonId] [nvarchar](32) NOT NULL,
    [SchoolYear] [smallint] NOT NULL,
    [SourceSystemDescriptor] [nvarchar](306) NOT NULL,
    [TermDescriptor] [nvarchar](306) NOT NULL,
    ObjectiveRatingLevelDescriptor [nvarchar](306) NULL,
    Comments NVARCHAR(1024) NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
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


CREATE TABLE [eppeta].[EvaluationElement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EducationOrganizationId] [bigint] NOT NULL,
	[EvaluationElementTitle] [nvarchar](255) NOT NULL,
	[EvaluationObjectiveTitle] [nvarchar](50) NOT NULL,
	[EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
	[EvaluationTitle] [nvarchar](50) NOT NULL,
	[PerformanceEvaluationTitle] [nvarchar](50) NOT NULL,
	[PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
	[SchoolYear] [smallint] NOT NULL,
	[TermDescriptor] [nvarchar](306) NOT NULL,
	[EvaluationTypeDescriptor] [nvarchar](306) NULL,
	[CreateDate] [datetime2](3) NOT NULL,
	[LastModifiedDate] [datetime2](3) NOT NULL,
	[EdFi_Id] nvarchar(50) NULL,
 CONSTRAINT [EvaluationElement_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [eppeta].[EvaluationElement] ADD  CONSTRAINT [EvaluationElement_DF_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [eppeta].[EvaluationElement] ADD  CONSTRAINT [EvaluationElement_DF_LastModifiedDate]  DEFAULT (getdate()) FOR [LastModifiedDate]
GO

CREATE TABLE eppeta.EvaluationElementRating (
    [EducationOrganizationId] [bigint] NOT NULL,
    [EvaluationDate] [datetime2](3) NOT NULL,
    [EvaluationElementTitle] [nvarchar](255) NOT NULL,
    [EvaluationObjectiveTitle] [nvarchar](50) NOT NULL,
    [EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
    [PersonId] [nvarchar](32) NOT NULL,
    [SchoolYear] [smallint] NOT NULL,
    [SourceSystemDescriptor] [nvarchar](306) NOT NULL,
    [TermDescriptor] [nvarchar](306) NOT NULL,
    EvaluationElementRatingLevelDescriptor [nvarchar](306) NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    UserId NVARCHAR(225) NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationElementRating_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.EvaluationElementRating ADD CONSTRAINT [EvaluationElementRating_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [eppeta].[EvaluationElementRating] WITH CHECK ADD CONSTRAINT [FK_EvaluationElementRating_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO


CREATE TABLE [eppeta].[EvaluationElementRatingResult](
    [EducationOrganizationId] [bigint] NOT NULL,
    [EvaluationDate] [datetime2](3) NOT NULL,
    [EvaluationElementTitle] [nvarchar](255) NOT NULL,
    [EvaluationObjectiveTitle] [nvarchar](50) NOT NULL,
    [EvaluationPeriodDescriptor] [nvarchar](306) NOT NULL,
    [EvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTitle] [nvarchar](50) NOT NULL,
    [PerformanceEvaluationTypeDescriptor] [nvarchar](306) NOT NULL,
    [PersonId] [nvarchar](32) NOT NULL,
    [SchoolYear] [smallint] NOT NULL,
    [SourceSystemDescriptor] [nvarchar](306) NOT NULL,
    [TermDescriptor] [nvarchar](306) NOT NULL,
    [Rating] [decimal](6, 3) NOT NULL,
    [RatingResultTitle] [nvarchar](50) NOT NULL,
    [ResultDatatypeTypeDescriptor] [nvarchar](306) NOT NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [EdFi_Id] NVARCHAR(50) NULL,
    [UserId] NVARCHAR(225) NOT NULL,
    [Id] INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT EvaluationElementRatingResult_PK PRIMARY KEY CLUSTERED (Id ASC)
  );
GO
ALTER TABLE [eppeta].[EvaluationElementRatingResult] ADD CONSTRAINT [EvaluationElementRatingResult_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [eppeta].[EvaluationElementRatingResult] WITH CHECK ADD CONSTRAINT [FK_EvaluationElementRatingResult_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [eppeta].[Users] ([Id])
GO
