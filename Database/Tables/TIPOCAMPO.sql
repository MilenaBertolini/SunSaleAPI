﻿CREATE TABLE [TIPOCAMPO] (
    [CODIGO] INT          DEFAULT ((0)) NOT NULL,
    [NOME]   VARCHAR (50) DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC, [NOME] ASC)
);

