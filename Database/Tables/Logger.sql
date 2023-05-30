create table Logger(
	Id int primary key,
	Descricao varchar(MAX) not null,
	Tipo int not null,
	StackTrace varchar(MAX),
	Created DateTime,
	Updated DateTime,
)