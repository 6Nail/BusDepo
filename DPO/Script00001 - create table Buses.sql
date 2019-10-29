create table Buss 
(
	[Id] int primary key identity(1,1),
	[Status] int  null,
	[Condition] nvarchar(MAX)  null,
	[MechanicId] int null,
)