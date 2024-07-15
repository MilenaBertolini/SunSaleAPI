create table Postagem(
	Id int identity(1000, 1) primary key,
	Created datetime not null,
	Updated datetime not null,
	IsActive char(1) default '1' not null,
	Descricao varchar(max) not null,
	DescricaoEn varchar(max) not null,
	TipoPostagem int,
	Capa varchar(255) not null,
	Curtidas int default 0 not null
)