﻿CREATE TABLE [ANEXOSQUESTOES] (
    [CODIGO]        INT             NOT NULL,
    [CODIGOQUESTAO] INT             DEFAULT ((0)) NOT NULL,
    [DATAREGISTRO]  DATETIME        DEFAULT ('') NOT NULL,
    [ANEXO]         VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

