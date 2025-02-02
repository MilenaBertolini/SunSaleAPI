﻿CREATE TABLE [OLEOSESSENCIAIS] (
    [CODIGO]              INT            NOT NULL,
    [CODIGOPRODUTO]       VARCHAR (20)   NULL,
    [NOME]                VARCHAR (300)  DEFAULT ('') NOT NULL,
    [TAMANHO]             VARCHAR (50)   NULL,
    [PRECOREGULAR]        VARCHAR (50)   NULL,
    [PRECOMEMBROS]        VARCHAR (50)   NULL,
    [PV]                  VARCHAR (20)   DEFAULT ('') NOT NULL,
    [MODOUSAR]            VARCHAR (4000) NULL,
    [DESCRICAO]           VARCHAR (4000) NULL,
    [PALAVRASCHAVES]      VARCHAR (4000) DEFAULT ('') NOT NULL,
    [COR]                 VARCHAR (300)  NULL,
    [BENEFICIOSPRIMARIOS] VARCHAR (4000) DEFAULT ('') NOT NULL,
    [DESCRICAOAROMATICA]  VARCHAR (4000) DEFAULT ('') NOT NULL,
    [METODOEXTRACAO]      VARCHAR (4000) NULL,
    [PARTEPLANTA]         VARCHAR (1000) NULL,
    [COMPONENTESQUIMICOS] VARCHAR (1000) NULL,
    [USOS]                VARCHAR (4000) NULL,
    [PRECAUCOES]          VARCHAR (4000) NULL,
    [TIPOPRODUTO]         VARCHAR (100)  DEFAULT ('OLEO') NOT NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

