﻿CREATE TABLE [CRUDFORMSINSTALADOR] (
    [CODIGO]    INT           NOT NULL,
    [VERSAO]    VARCHAR (10)  DEFAULT ('') NOT NULL,
    [CREATED]   DATETIME      DEFAULT (((2022)-(12))-(20)) NOT NULL,
    [DIRETORIO] VARCHAR (300) DEFAULT ('') NOT NULL,
    [ATIVO]     VARCHAR (1)   DEFAULT ('1') NOT NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

