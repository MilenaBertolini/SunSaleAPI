﻿CREATE TABLE [CONSULTAS] (
    [CODIGO]       INT            NOT NULL,
    [NOMECONSULTA] VARCHAR (100)  DEFAULT ('') NOT NULL,
    [CONSULTA]     VARCHAR (4000) DEFAULT ('') NOT NULL,
    [CREATED]      VARCHAR (20)   DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

