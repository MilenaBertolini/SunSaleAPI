create table WhosThatPokemonResult(
	Id int identity(1000, 1) primary key,
	Nome varchar(255),
	Created Datetime not null,
	Updated DateTime not null,
	Tempo int default 0,
	Acertos int not null,
	Erros int not null,
	Kanto tinyint default 0 not null,
	Johto tinyint default 0 not null,
	Hoenn tinyint default 0 not null,
	Sinnoh tinyint default 0 not null,
	Unova tinyint default 0 not null,
	Kalos tinyint default 0 not null,
	Alola tinyint default 0 not null,
	Paldea tinyint default 0 not null
)