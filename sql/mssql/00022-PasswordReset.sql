-- SPDX-License-Identifier: Apache-2.0
-- Licensed to the Ed-Fi Alliance under one or more agreements.
-- The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
-- See the LICENSE and NOTICES files in the project root for more information.

CREATE TABLE [eppeta].PasswordReset (
    Id INT IDENTITY(1,1) NOT NULL,
    UserId [nvarchar](225) NOT NULL,
    Token [nvarchar](300) NOT NULL,
    ExpirationTime DATETIME NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (UserId) REFERENCES [eppeta].[Users](Id)
);
