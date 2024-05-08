ALTER TABLE Dog_Breed DROP CONSTRAINT FKDog_Breed732075;
ALTER TABLE Daily_routine DROP CONSTRAINT FKDaily_rout691654;
ALTER TABLE Adoption_request DROP CONSTRAINT FKAdoption_r875844;
ALTER TABLE Adoption_request DROP CONSTRAINT FKAdoption_r376637;
ALTER TABLE Dog_Color DROP CONSTRAINT FKDog_Color318530;
ALTER TABLE Dog_Color DROP CONSTRAINT FKDog_Color998865;
ALTER TABLE Todo_Item DROP CONSTRAINT FKTodo_Item503662;
ALTER TABLE Routine_item_in_Shelter_ DROP CONSTRAINT FKRoutine_it190235;
ALTER TABLE Routine_item_in_Shelter_ DROP CONSTRAINT FKRoutine_it352508;
ALTER TABLE Shelter DROP CONSTRAINT FKShelter711890;
ALTER TABLE Optional_adopter DROP CONSTRAINT FKOptional_a86884;
ALTER TABLE Volunteer_Shelter DROP CONSTRAINT FKVolunteer_318949;
ALTER TABLE Volunteer_Shelter DROP CONSTRAINT FKVolunteer_236131;
ALTER TABLE Routine_item_in_Shelter_ DROP CONSTRAINT FKRoutine_it74644;
ALTER TABLE Characteristics_Dog DROP CONSTRAINT FKCharacteri993916;
ALTER TABLE Characteristics_Dog DROP CONSTRAINT FKCharacteri784397;
ALTER TABLE Daily_routine DROP CONSTRAINT checks;
ALTER TABLE Cell DROP CONSTRAINT Contained;
ALTER TABLE [File] DROP CONSTRAINT has;
ALTER TABLE Dog DROP CONSTRAINT [in];
ALTER TABLE Dog_Breed DROP CONSTRAINT [is a];
ALTER TABLE UserAddress DROP CONSTRAINT [is in];
ALTER TABLE Shelter DROP CONSTRAINT manages;
ALTER TABLE Image DROP CONSTRAINT [of];
DROP TABLE Adoption_request;
DROP TABLE Breed;
DROP TABLE Cell;
DROP TABLE Characteristics;
DROP TABLE Characteristics_Dog;
DROP TABLE City;
DROP TABLE Color;
DROP TABLE [Daily routine item];
DROP TABLE Daily_routine;
DROP TABLE Dog;
DROP TABLE Dog_Breed;
DROP TABLE Dog_Color;
DROP TABLE [File];
DROP TABLE Image;
DROP TABLE Optional_adopter;
DROP TABLE Routine_item_in_Shelter_;
DROP TABLE Shelter;
DROP TABLE ShelterAdmin;
DROP TABLE Todo_Item;
DROP TABLE UserAddress;
DROP TABLE Volunteer;
DROP TABLE Volunteer_Shelter;

drop table Adoption_request
CREATE TABLE Adoption_request
(
  RequestID int IDENTITY(1, 1) NOT NULL,
  Optional_adopterPhoneNumber char(10) NOT NULL,
  SentDate Date NULL,
  Status nvarchar(5) NULL,
  DogNumberId int NOT NULL,
  Deleted bit DEFAULT 'false' NULL,
  PRIMARY KEY (RequestID)
);
CREATE TABLE Breed
(
  Breed nvarchar(20) NOT NULL,
  PRIMARY KEY (Breed)
);

CREATE TABLE Cell
(
  ShelterNumber int NOT NULL,
  Number int NOT NULL,
  capacity int NOT NULL,
  id int IDENTITY(1, 1) NOT NULL,
  PRIMARY KEY (id)
);
CREATE TABLE Characteristics
(
  id int IDENTITY(0, 1) NOT NULL,
  attribute nvarchar(20) NOT NULL,
  PRIMARY KEY (id)
);
CREATE TABLE Characteristics_Dog
(
  Characteristicsid int NOT NULL,
  DogNumberId int NOT NULL,
  PRIMARY KEY (Characteristicsid, 
  DogNumberId)
);
CREATE TABLE City
(
  CityName nvarchar(12) NOT NULL,
  Region nvarchar(20) NULL,
  PRIMARY KEY (CityName)
);



CREATE TABLE Color
(
  ColorName nvarchar(20) NOT NULL,
  PRIMARY KEY (ColorName)
);
CREATE TABLE [Daily routine item]
(
  ItemID int IDENTITY(0, 1) NOT NULL,
  ShelterNumber varchar(10) NOT NULL,
  Type varchar(10) NULL,
  PRIMARY KEY (ItemID)
);
CREATE TABLE Daily_routine
(
  routineId int IDENTITY(0, 1) NOT NULL,
  FiiledDate datetime DEFAULT GETDATE() NOT NULL,
  Note nvarchar(2048) NULL,
  DogNumberId int NOT NULL,
  VolunteerPhoneNumber char(10) NULL,
  ShelterNumber int NULL,
  Deleted bit DEFAULT 'false' NULL,
  PRIMARY KEY (routineId)
);
CREATE TABLE Dog
(
  ChipNumber varchar(15) NOT NULL UNIQUE,
  NumberId int IDENTITY(0, 1) NOT NULL,
  Name nvarchar(20) NOT NULL,
  DateOfBirth date NULL,
  Gender nvarchar(5) NOT NULL CHECK(Gender in ('���','����')),
  EntrandeDate date NOT NULL,
  IsAdoptable bit DEFAULT 'false' NOT NULL,
  [Size] varchar(20) NOT NULL CHECK(Size in ('���','���-������','������','������-����','����')),
  Adopted bit DEFAULT 'false' NOT NULL,
  IsReturned bit DEFAULT 'false' NOT NULL,
  Cellid int NULL,
  Note nvarchar(1000),
  PRIMARY KEY (NumberId)
);

use [igroup154_test1]
alter table Dog add Note nvarchar(1000)

CREATE TABLE Dog_Breed
(
  Breed nvarchar(20) NOT NULL,
  DogNumberId int NOT NULL,
  PRIMARY KEY (Breed, 
  DogNumberId)
);
CREATE TABLE Dog_Color
(
  DogNumberId int NOT NULL,
  ColorName nvarchar(20) NOT NULL,
  PRIMARY KEY (DogNumberId, 
  ColorName)
);
CREATE TABLE [File]
(
  FileId int IDENTITY(0, 1) NOT NULL,
  Type varchar(10) NULL,
  Url nvarchar(1000) NULL,
  FileName nvarchar(12) NOT NULL,
  DogNumberId int NOT NULL,
  PRIMARY KEY (FileId)
);

CREATE TABLE Image
(
  DogNumberId int NOT NULL,
  ImageId int IDENTITY(1, 1) NOT NULL,
  Path nvarchar(1000) NOT NULL,
  isProfile bit DEFAULT 'false',
  PRIMARY KEY (ImageId)
);
CREATE TABLE Optional_adopter
(
  PhoneNumber char(10) NOT NULL,
  Email varchar(40) NOT NULL UNIQUE,
  FirstName nvarchar(12) NOT NULL,
  LastName nvarchar(12) NOT NULL,
  DateOfBirth date NULL,
  HouseMembers nvarchar(20) NULL,
  DogsPlace nvarchar(20) NULL,
  AdditionalPets nvarchar(20) NULL,
  Experience nvarchar(20) NULL,
  Note nvarchar(2048) NULL,
  Addressid int NOT NULL,
  Deleted bit DEFAULT 'false' NULL,
  PRIMARY KEY (PhoneNumber)
);
CREATE TABLE Routine_item_in_Shelter_
(
  ShelterNumber int NOT NULL,
  [DailyRoutine itemID] int NOT NULL,
  DailyRoutineId int NOT NULL,
  PRIMARY KEY (ShelterNumber, 
  [DailyRoutine itemID])
);
CREATE TABLE Shelter
(
  ShelterNumber int IDENTITY(1, 1) NOT NULL,
  AdminPhoneNumber char(10) NOT NULL,
  FacebookUserName nvarchar(30) NULL,
  FacebookPassword nvarchar(20) NULL,
  InstegramUserName nvarchar(30) NULL,
  InstegramPassword nvarchar(20) NULL,
  TimeToReport time(7) NULL,
  [Name] nvarchar(12) NOT NULL,
  PhotoUrl nvarchar(1000) NULL,
  AddressId int NOT NULL,
  Deleted bit DEFAULT 'false' NULL,
  PRIMARY KEY (ShelterNumber)
);



CREATE TABLE ShelterAdmin
(
  PhoneNumber char(10) NOT NULL,
  UserName nvarchar(12) NOT NULL,
  [Password] varchar(20),
  Email varchar(40) NULL UNIQUE,
  FirstName nvarchar(12) NOT NULL,
  LastName nvarchar(12) NOT NULL,
  Deleted bit DEFAULT 'false' NULL,
  PRIMARY KEY (PhoneNumber)
);
CREATE TABLE Todo_Item
(
  TodoId int IDENTITY(0, 1) NOT NULL,
  Done bit DEFAULT 'false' NOT NULL,
  DoDate date NOT NULL CHECK(DoDate>=GETDATE()),
  text nvarchar(50) NOT NULL,
  Repetition int NULL,
  ShelterNumber int NOT NULL,
  PRIMARY KEY (TodoId)
);
CREATE TABLE UserAddress
(
  Id int IDENTITY(0, 1) NOT NULL,
  HouseNumber int NOT NULL,
  StreetName nvarchar(12) NOT NULL,
  Cityid int NOT NULL,
  PRIMARY KEY (Id)
);
CREATE TABLE Volunteer
(
  PhoneNumber char(10) NOT NULL,
  Email varchar(40) NULL UNIQUE,
  FirstName nvarchar(12) NOT NULL,
  LastName nvarchar(12) NOT NULL,
  Deleted bit DEFAULT 'false' NULL,
  PRIMARY KEY (PhoneNumber)
);
CREATE TABLE Volunteer_Shelter
(
  volunteerPhoneNumber char(10) NOT NULL,
  ShelterNumber int NOT NULL,
  PRIMARY KEY (volunteerPhoneNumber, 
  ShelterNumber)
);
ALTER TABLE Dog_Breed ADD CONSTRAINT FKDog_Breed732075 FOREIGN KEY (Breed) REFERENCES Breed (Breed);
ALTER TABLE Daily_routine ADD CONSTRAINT FKDaily_rout691654 FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE Adoption_request ADD CONSTRAINT FKAdoption_r875844 FOREIGN KEY (Optional_adopterPhoneNumber) REFERENCES Optional_adopter (PhoneNumber);
ALTER TABLE Adoption_request ADD CONSTRAINT FKAdoption_r376637 FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE Dog_Color ADD CONSTRAINT FKDog_Color318530 FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE Dog_Color ADD CONSTRAINT FKDog_Color998865 FOREIGN KEY (ColorName) REFERENCES Color (ColorName);
ALTER TABLE Todo_Item ADD CONSTRAINT FKTodo_Item503662 FOREIGN KEY (ShelterNumber) REFERENCES Shelter (ShelterNumber);
ALTER TABLE Routine_item_in_Shelter_ ADD CONSTRAINT FKRoutine_it190235 FOREIGN KEY (ShelterNumber) REFERENCES Shelter (ShelterNumber);
ALTER TABLE Routine_item_in_Shelter_ ADD CONSTRAINT FKRoutine_it352508 FOREIGN KEY ([DailyRoutine itemID]) REFERENCES [Daily routine item] (ItemID);
ALTER TABLE Shelter ADD CONSTRAINT FKShelter711890 FOREIGN KEY (AddressId) REFERENCES UserAddress (Id);
ALTER TABLE Optional_adopter ADD CONSTRAINT FKOptional_a86884 FOREIGN KEY (Addressid) REFERENCES UserAddress (Id);
ALTER TABLE Volunteer_Shelter ADD CONSTRAINT FKVolunteer_318949 FOREIGN KEY (volunteerPhoneNumber) REFERENCES Volunteer (PhoneNumber);
ALTER TABLE Volunteer_Shelter ADD CONSTRAINT FKVolunteer_236131 FOREIGN KEY (ShelterNumber) REFERENCES Shelter (ShelterNumber);
ALTER TABLE Routine_item_in_Shelter_ ADD CONSTRAINT FKRoutine_it74644 FOREIGN KEY (DailyRoutineId) REFERENCES Daily_routine (routineId);
ALTER TABLE Characteristics_Dog ADD CONSTRAINT FKCharacteri993916 FOREIGN KEY (Characteristicsid) REFERENCES Characteristics (id);
ALTER TABLE Characteristics_Dog ADD CONSTRAINT FKCharacteri784397 FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE Daily_routine ADD CONSTRAINT checks FOREIGN KEY (VolunteerPhoneNumber, ShelterNumber) REFERENCES Volunteer_Shelter (volunteerPhoneNumber, ShelterNumber);
ALTER TABLE Cell ADD CONSTRAINT Contained FOREIGN KEY (ShelterNumber) REFERENCES Shelter (ShelterNumber);
ALTER TABLE [File] ADD CONSTRAINT has FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE Dog ADD CONSTRAINT [in] FOREIGN KEY (Cellid) REFERENCES Cell (id);
ALTER TABLE Dog_Breed ADD CONSTRAINT [is a] FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE UserAddress ADD CONSTRAINT [is in] FOREIGN KEY (City) REFERENCES City (CityName);
ALTER TABLE Shelter ADD CONSTRAINT manages FOREIGN KEY (AdminPhoneNumber) REFERENCES ShelterAdmin (PhoneNumber);
ALTER TABLE Image ADD CONSTRAINT [of] FOREIGN KEY (DogNumberId) REFERENCES Dog (NumberId);
ALTER TABLE Adoption_request ADD CONSTRAINT UC_adoption UNIQUE (DogNumberId,Optional_adopterPhoneNumber)



insert into Breed
  (Breed)
values
  ('������'),
  ('����� ������'),
  ('������ �����'),
  ('�������� ����'),
  ('�����'),
  ('���'),
  ('����'),
  ('�����'),
  ('����� ����'),
  ('���� �����'),
  ('����-��'),
  ('���� �������'),
  ('����'),
  ('���� ������'),
  ('������ �����'),
  ('����'),
  ('��������'),
  ('������ ����'),
  ('��������'),
  ('���� ����'),
  ('�����'),
  ('����� ������'),
  ('���������'),
  ('���� �����'),
  ('������'),
  ('�� ���� ����'),
  ('���� ����� ������'),
  ('�����'),
  ('��� �����')



