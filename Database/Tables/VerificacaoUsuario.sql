create table VerificacaoUsuario(
	codigo int primary key,
	GuidText varchar(64) not null,
	CodigoUsuario int not null,
	IsActive char(1) default '1',
	Created DateTime,
	Updated DateTime
);