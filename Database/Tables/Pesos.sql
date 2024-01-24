CREATE TABLE [Pesos]
(
	Id int identity(1000, 1) primary key,
	Faculdade varchar(500) not null,
	Curso varchar(500) not null,
	Turno varchar(250) not null,
	Materia varchar(250) not null,
	Peso decimal(10,2) not null
)