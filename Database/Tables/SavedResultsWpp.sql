create table SavedResultsWpp(
	Id int identity(1000, 1) primary key,
	Created datetime not null,
	Updated datetime not null,
	IsActive char(1) default '1' not null,
	[Json] varchar(max) not null,
)