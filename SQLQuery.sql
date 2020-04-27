CREATE TABLE [dbo].[Recipient] (
    [ID] int NOT NULL,
    [Email]  NVARCHAR (MAX) NOT NULL,
    [Name]   NVARCHAR (MAX) NOT NULL
);
ALTER TABLE [dbo].[Recipient]   
ADD CONSTRAINT PK_Recipient_ID PRIMARY KEY CLUSTERED (ID);
