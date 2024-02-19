CREATE TABLE [dbo].[QuestoesAvaliacao]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[CreatedOn]         DATETIME not null,
    [CreatedBy]         INT not null,
	[IdAvaliacao]		int not null,
	[IdQuestao]			int not null,
	[NotaQuestao]		decimal(10,2) default 1
)
