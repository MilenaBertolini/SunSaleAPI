create table [RecuperaSenha]
(
	Code int primary key,
	Created datetime,
	Updated datetime,
	Guid varchar(100) not null,
	EmailUser varchar(300) not null,
	Validated char(1) default '0' not null
)