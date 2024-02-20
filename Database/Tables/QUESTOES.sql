CREATE TABLE [QUESTOES] (
    [CODIGO]            INT            NOT NULL,
    [DATAREGISTRO]      DATETIME       NOT NULL,
    [CAMPOQUESTAO]      VARCHAR (8000) DEFAULT ('') NOT NULL,
    [OBSERVACAOQUESTAO] VARCHAR (8000) DEFAULT ('') NOT NULL,
    [MATERIA]           VARCHAR (300)  DEFAULT ('') NOT NULL,
    [CODIGOPROVA]       INT            DEFAULT ((0)) NOT NULL,
    [NUMEROQUESTAO]     VARCHAR (20)   NULL,
    [ativo]             CHAR (1)       DEFAULT ('1') NOT NULL,
    [UpdatedBy]         INT,
    [UpdatedOn]         DATETIME,
    [CreatedBy]         INT,
    [Assunto] VARCHAR(255) NULL, 
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

