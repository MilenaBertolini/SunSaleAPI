﻿CREATE TABLE [EMPRESAFORDEV] (
    [NOME]         VARCHAR (200) NULL,
    [CNPJ]         VARCHAR (15)  DEFAULT ('') NOT NULL,
    [IE]           VARCHAR (50)  DEFAULT ('') NOT NULL,
    [DATAABERTURA] VARCHAR (12)  DEFAULT ('') NOT NULL,
    [SITE]         VARCHAR (500) DEFAULT ('') NOT NULL,
    [EMAIL]        VARCHAR (500) DEFAULT ('') NOT NULL,
    [CEP]          VARCHAR (10)  DEFAULT ('') NOT NULL,
    [ENDERECO]     VARCHAR (100) DEFAULT ('') NOT NULL,
    [NUMERO]       VARCHAR (20)  DEFAULT ('') NOT NULL,
    [BAIRRO]       VARCHAR (500) DEFAULT ('') NOT NULL,
    [CIDADE]       VARCHAR (500) DEFAULT ('') NOT NULL,
    [ESTADO]       CHAR (3)      DEFAULT ('SP') NOT NULL,
    [TELEFONEFIXO] VARCHAR (30)  DEFAULT ('') NOT NULL,
    [CELULAR]      VARCHAR (30)  DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([CNPJ] ASC)
);

