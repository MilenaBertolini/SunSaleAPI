create table Simulados(
	Codigo int primary key,
	Respostas varchar(MAX) not null,
	Created DateTime,
	CodigoUsuario int not null,
	CodigoProva int not null,
	QuantidadeQuestoes int not null,
	QuantidadeCertas int not null,
	Tempo int not null
)