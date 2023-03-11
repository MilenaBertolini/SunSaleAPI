﻿CREATE TABLE [PESSOASFORDEV] (
    [NOME]           VARCHAR (2000) DEFAULT ('') NOT NULL,
    [IDADE]          INT            DEFAULT ((0)) NOT NULL,
    [CPF]            VARCHAR (12)   DEFAULT ('') NOT NULL,
    [RG]             VARCHAR (12)   DEFAULT ('') NOT NULL,
    [DATANASCIMENTO] VARCHAR (10)   DEFAULT ('') NOT NULL,
    [SEXO]           VARCHAR (20)   DEFAULT ('Masculino') NOT NULL,
    [SIGNO]          VARCHAR (30)   DEFAULT ('') NOT NULL,
    [MAE]            VARCHAR (2000) DEFAULT ('') NOT NULL,
    [PAI]            VARCHAR (2000) DEFAULT ('') NOT NULL,
    [EMAIL]          VARCHAR (500)  DEFAULT ('') NOT NULL,
    [SENHA]          VARCHAR (50)   DEFAULT ('') NOT NULL,
    [CEP]            VARCHAR (8)    DEFAULT ('') NOT NULL,
    [ENDERECO]       VARCHAR (1000) DEFAULT ('') NOT NULL,
    [NUMERO]         INT            DEFAULT ((0)) NOT NULL,
    [BAIRRO]         VARCHAR (500)  DEFAULT ('') NOT NULL,
    [CIDADE]         VARCHAR (1000) DEFAULT ('') NOT NULL,
    [ESTADO]         CHAR (2)       DEFAULT ('MG') NOT NULL,
    [TELEFONEFIXO]   VARCHAR (50)   DEFAULT ('') NOT NULL,
    [CELULAR]        VARCHAR (50)   DEFAULT ('') NOT NULL,
    [ALTURA]         VARCHAR (10)   DEFAULT ('') NOT NULL,
    [PESO]           INT            DEFAULT ((0)) NOT NULL,
    [TIPOSANGUINEO]  VARCHAR (3)    DEFAULT ('') NOT NULL,
    [CORFAVORITA]    VARCHAR (30)   DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([CPF] ASC)
);

