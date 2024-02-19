CREATE TABLE [dbo].[Avaliacao]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [CreatedOn]         DATETIME not null,
    [UpdatedOn]         DATETIME not null,
    [CreatedBy]         INT not null,
	[UpdatedBy]         INT not null,
    [Nome]              NVARCHAR(255) not null,
    [Orientação]        NVARCHAR(MAX),
    [IsActive]          char(1) default '1' not null,
    [IsPublic]          char(1) default '1' not null,
    [Key]               nvarchar(64) 
)
