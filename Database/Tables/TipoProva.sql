create table TipoProva(
	Codigo int primary key,
	Descricao Varchar(255) not null,
	Created DateTime,
	Updated DateTime,
	CreatedBy int,
	UpdatedBy int,
	IsActive char(1) default '1'
);