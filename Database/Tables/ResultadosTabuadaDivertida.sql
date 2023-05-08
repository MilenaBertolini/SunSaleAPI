CREATE TABLE [dbo].[ResultadosTabuadaDivertida]
(
	Codigo int primary key,
	Nome varchar(300) not null,
	Tempo int not null,
	NumeroAcertos int not null,
	Created DateTime,
	Tipo char(1),
	NumeroQuestoes int
)
