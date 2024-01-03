-- SPDX-License-Identifier: Apache-2.0
-- Licensed to the Ed-Fi Alliance under one or more agreements.
-- The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
-- See the LICENSE and NOTICES files in the project root for more information.

CREATE TABLE eppeta.Candidate (
    FirstName NVARCHAR(64) NOT NULL,
    LastName NVARCHAR(64) NOT NULL,
    [PersonId] [nvarchar](32) NOT NULL,
    [SourceSystemDescriptor] [nvarchar](306) NOT NULL,
    [CreateDate] [datetime2](3) NOT NULL,
    [LastModifiedDate] [datetime2](3) NOT NULL,
    EdFi_Id NVARCHAR(50) NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    CONSTRAINT Candidate_PK PRIMARY KEY CLUSTERED (Id ASC)
);
GO
ALTER TABLE eppeta.Candidate ADD CONSTRAINT [Candidate_DF_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE eppeta.Candidate ADD CONSTRAINT [Candidate_DF_LastModifiedDate] DEFAULT (getdate()) FOR [LastModifiedDate]
GO
