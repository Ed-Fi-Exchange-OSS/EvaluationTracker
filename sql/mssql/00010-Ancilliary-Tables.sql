CREATE TABLE [eppeta].[Status] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [StatusText] [nvarchar](15) NOT NULL
    CONSTRAINT Status_PK PRIMARY KEY CLUSTERED (Id ASC)
    );
INSERT INTO [eppeta].[Status] ([StatusText]) VALUES ('Not Uploaded'),('Uploaded'),('Failed');


