﻿CREATE TABLE [RESPOSTASUSUARIOS] (
    [CODIGO]         INT      NOT NULL,
    [CODIGOUSUARIO]  INT      DEFAULT ((0)) NOT NULL,
    [CODIGORESPOSTA] INT      DEFAULT ((0)) NOT NULL,
    [DATARESPOSTA]   DATETIME NOT NULL,
    [CodigoQuestao]  INT,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

