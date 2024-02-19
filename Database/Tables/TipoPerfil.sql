CREATE TABLE [dbo].[TipoPerfil]
(
	[Id] [char](1) NOT NULL PRIMARY KEY,
	[Descricao] [nvarchar](255) not null,
	[Created] [DateTime] not null,
	[Updated] [DateTime] not null
)
