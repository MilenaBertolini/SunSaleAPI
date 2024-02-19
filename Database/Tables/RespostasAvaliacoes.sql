CREATE TABLE [dbo].[RespostasAvaliacoes]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[CreatedOn]         DATETIME not null,
	[UpdatedOn]         DATETIME not null,
	[CreatedBy]         INT not null,
	[UpdatedBy]         INT not null,
	[IdAvaliacao]		int not null,
	[IdQuestao]			int not null,
	[IdResposta]		int not null,
)
