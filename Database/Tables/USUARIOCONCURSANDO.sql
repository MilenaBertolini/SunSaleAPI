CREATE TABLE [USUARIOCONCURSANDO] (
    [CODIGO]         INT           NOT NULL,
    [DATANASCIMENTO] DATE          NOT NULL,
    [EMAIL]          VARCHAR (300) NOT NULL,
    [NOME]           VARCHAR (200) NOT NULL,
    [PASSWORD]       VARCHAR (100) NOT NULL,
    [ADMIN]          CHAR (1)      DEFAULT ('0') NOT NULL,
    [created]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [BOT]            CHAR (1)      DEFAULT ('0') NOT NULL,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

