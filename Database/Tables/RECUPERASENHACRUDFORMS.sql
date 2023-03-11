CREATE TABLE [RECUPERASENHACRUDFORMS] (
    [codigo]     INT           NOT NULL,
    [created]    DATETIME      NOT NULL,
    [guid]       VARCHAR (64)  NULL,
    [email]      VARCHAR (300) NULL,
    [recuperado] CHAR (1)      DEFAULT ('0') NOT NULL,
    [updated]    DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([codigo] ASC)
);

