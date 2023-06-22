-- SPDX-License-Identifier: Apache-2.0
-- Licensed to the Ed-Fi Alliance under one or more agreements.
-- The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
-- See the LICENSE and NOTICES files in the project root for more information.

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

IF (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DeployJournal' AND TABLE_SCHEMA = 'eppeta') IS NULL
BEGIN
    CREATE TABLE [eppeta].[DeployJournal](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ScriptName] [nvarchar](255) NOT NULL,
        [Applied] [datetime] NOT NULL,
    CONSTRAINT [PK_DeployJournal_Id] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO
