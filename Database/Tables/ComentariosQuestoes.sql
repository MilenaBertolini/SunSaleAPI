create table ComentariosQuestoes(
	Codigo int primary key,
	Comentario varchar(MAX) not null,
	CodigoUsuario int not null,
	CodigoQuestao int not null,
	IsActive char(1) default '1' not null,
	Created DateTime,
	Updated DateTime
);