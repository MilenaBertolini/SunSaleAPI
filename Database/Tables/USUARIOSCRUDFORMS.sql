CREATE TABLE [USUARIOSCRUDFORMS] (
    [CODIGO]        INT           NOT NULL,
    [LOGIN]         VARCHAR (50)  DEFAULT ('') NOT NULL,
    [SENHA]         VARCHAR (20)  DEFAULT ('') NOT NULL,
    [ADMINISTRADOR] CHAR (1)      DEFAULT ('0') NOT NULL,
    [DESENVOLVEDOR] CHAR (1)      DEFAULT ('0') NOT NULL,
    [EMAIL]         VARCHAR (200) DEFAULT ('') NOT NULL,
    [USUARIOPAI]    INT           DEFAULT ((-1)) NOT NULL,
    Nome            varchar(100),
    Created         DateTime      Default Getdate() not null,
    Updated         DateTime      Default Getdate() not null,
    PRIMARY KEY CLUSTERED ([CODIGO] ASC)
);

