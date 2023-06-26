create table LicencasSunSalePro(
	Codigo int primary key,
	Licenca varchar(100) not null,
	IsActive char(1) default '1' not null,
	Created DateTime,
	Updated DateTime)