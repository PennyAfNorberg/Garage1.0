use Garage
go

create schema tests
go


 create table Garage.tests.Garage_Garage
 (
	IsNotNull bit not null,
	Capasity int null,
	ExpectedResult bit not null,
	CausesException bit not null
 )

  create table Garage.tests.Garage_GetAllVehicles
  (
	IsNotNull bit not null,
	Capasity int null,
	ExpectedResult bit not null,
	CausesException bit not null
  )

  create table Garage.tests.Garage_Remove
  (
	IsNotNull bit not null,
	Capasity int null,
	ExpectedResult bit not null,
	CausesException bit not null
  )

   create table Garage.tests.Garage_Resize
  (
	IsNotNull bit not null,
	Capasity int null,
	NewCapasity int null,
	ExpectedResult bit not null,
	CausesException bit not null
  )

  create table Garage.tests.Garage_Add
  (
	IsNotNull bit not null,
	Capasity int null,
	ExpectedResult bit not null,
	CausesException bit not null
  )