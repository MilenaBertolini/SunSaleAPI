CREATE TABLE [PROVA] (
    [CODIGO]             INT             NOT NULL,
    [NOMEPROVA]          VARCHAR (500)   DEFAULT ('') NOT NULL,
    [LOCAL]              VARCHAR (500)   DEFAULT ('') NOT NULL,
    [TIPOPROVA]          VARCHAR (100)   DEFAULT ('') NOT NULL,
    [DATAAPLICACAO]      VARCHAR (10)    DEFAULT ('') NOT NULL,
    [PROVA]              VARBINARY (MAX) NULL,
    [GABARITO]           VARBINARY (MAX) NULL,
    [OBSERVACAOPROVA]    VARCHAR (3000)  NULL,
    [OBSERVACAOGABARITO] VARCHAR (2000)  DEFAULT ('') NOT NULL,
    [DATAREGISTRO]       DATETIME        NOT NULL,
    [BANCA]              VARCHAR (1000)  NULL,
    [UpdatedBy]         INT,
    [UpdatedOn]         DATETIME,
    [CreatedBy]         INT,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

