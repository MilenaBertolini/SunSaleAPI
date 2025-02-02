﻿CREATE TABLE [RESPOSTASQUESTOES] (
    [CODIGO]             INT            NOT NULL,
    [CODIGOQUESTAO]      INT            DEFAULT ((0)) NOT NULL,
    [DATAREGISTRO]       DATETIME       DEFAULT ('') NOT NULL,
    [TEXTORESPOSTA]      VARCHAR (8000) DEFAULT ('') NOT NULL,
    [CERTA]              CHAR (1)       DEFAULT ('0') NOT NULL,
    [OBSERVACAORESPOSTA] VARCHAR (8000) NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC),
    FOREIGN KEY (CODIGOQUESTAO) REFERENCES QUESTOES(CODIGO)
);

