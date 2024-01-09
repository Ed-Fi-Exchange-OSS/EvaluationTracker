CREATE TABLE [eppeta].[Status] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [StatusText] [nvarchar](50) NOT NULL
    CONSTRAINT Status_PK PRIMARY KEY CLUSTERED (Id ASC)
    );
INSERT INTO [eppeta].[Status] ([StatusText]) VALUES ('Ready for Review'),('Review Approved, transferred to ODS/API'),('Review Approved, transfer to ODS/API failed');


