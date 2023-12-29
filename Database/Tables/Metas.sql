create table Metas(
	Id int identity(1000, 1) primary key,
	Created datetime not null,
	Updated datetime not null,
	IsActive char(1) default '1' not null,
	Meta varchar(max) not null,
	Email varchar(255) not null,
	DataObjetivo datetime not null
)