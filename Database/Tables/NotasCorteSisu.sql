﻿CREATE TABLE [dbo].[NotasCorteSisu]
(
	[Id] BIGINT NOT NULL PRIMARY KEY,
	[Year] Int not null,
	[NumeroEdicao] char(1) not null,
	[CodigoInstituicaoEnsino] int not null,
	[NomeInstituicao] varchar(500) not null,
	[SiglaInstituicao] varchar(255) not null,
	[OrganizacaoAcademica] varchar(255) not null,
	[CategoriaOrganizacao] varchar(255) not null,
	[NomeCampus] varchar(500) not null,
	[NomeMunicipioCampus] varchar(500) not null,
	[UFCampus] varchar(2) not null,
	[RegiaoCampus] varchar(50) not null,
	[CodigoCurso] bigInt not null,
	[NomeCurso] varchar(500) not null,
	[DescricaoGrau] varchar(500) not null,
	[Turno] varchar(100) not null,
	[ModoConcorrencia] varchar(500) not null,
	[DesricaoModoConcorrencia] varchar(1000) not null,
	[BonusAcaoAfirmativa] decimal not null,
	[QuantidadeVagas] int not null,
	[NotaCorte] decimal not null,
	[QuantidadeInscricoes] int not null,
)
