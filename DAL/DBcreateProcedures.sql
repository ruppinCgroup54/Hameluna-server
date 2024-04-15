USE [igroup154_test1]
GO



Drop procedure if exists CityTableIUD
GO
create PROCEDURE CityTableIUD @Id int,  @CityName NVARCHAR(12), @Region nvarchar(30),@StatementType varchar(10)
AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
			INSERT INTO City
					  (CityName, 
					  Region) 

			VALUES 
					  (@CityName, 
					  @Region)
					  
			SELECT SCOPE_IDENTITY() 

        END

      IF @StatementType = 'Select'
        BEGIN
            SELECT CityName, Region, Id 
			FROM City;
        END

      IF @StatementType = 'Update'
        BEGIN
            UPDATE City SET 
			  CityName = @CityName, 
			  Region = @Region 
			WHERE
			  Id = @Id;
        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           DELETE FROM City 
			WHERE Id = @Id;
        END
  END
GO





Drop procedure if exists BreedTableIUD
GO
create PROCEDURE BreedTableIUD 
				@Breed nvarchar(12), 
				@StatementType varchar(10)
AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
				INSERT INTO Breed
						  (Breed) 
						VALUES 
						  (@Breed)
			
        END

      IF @StatementType = 'Select'
        BEGIN
            SELECT ua.Id, ua.HouseNumber, ua.StreetName, c.CityName ,c.Region
			FROM UserAddress ua inner join City c on ua.Cityid = c.Id;

        END

      IF @StatementType = 'Update'
        BEGIN
			UPDATE Breed SET 
			Breed = @Breed
			WHERE
			  Breed = @Breed

        END

	  IF @StatementType = 'Delete'
        BEGIN
        DELETE FROM Breed 
		WHERE Breed = @Breed;


        END
  END
GO


Drop procedure if exists DogTableIUD
GO
create PROCEDURE DogTableIUD 
				@ChipNumber varchar(15), 
				  @Cellnumber int, 
				  @NumberId int, 
				  @Name nvarchar(20), 
				  @DateOfBirth date, 
				  @Gender char(1), 
				  @EntrandeDate date, 
				  @IsAdoptable bit, 
				  @Size varchar(20), 
				  @Adopted bit, 
				  @IsReturned bit, 
				  @Cellid int, 
				  @Deleted bit,
				  @StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
			INSERT INTO Dog
				  (ChipNumber, 
				  NumberId, 
				  [Name], 
				  DateOfBirth, 
				  Gender, 
				  EntrandeDate, 
				  IsAdoptable, 
				  [Size], 
				  Adopted, 
				  IsReturned, 
				  Cellid
				  ) 
				VALUES 
				  (@ChipNumber, 
				  @NumberId, 
				  @Name, 
				  @DateOfBirth, 
				  @Gender, 
				  @EntrandeDate, 
				  @IsAdoptable, 
				  @Size, 
				  @Adopted, 
				  @IsReturned, 
				  @Cellid
				  ) ;

				SELECT SCOPE_IDENTITY() 
			
        END

      IF @StatementType = 'Select'
        BEGIN
            SELECT ChipNumber, NumberId, [Name], DateOfBirth, Gender, EntrandeDate, IsAdoptable, [Size], Adopted, IsReturned, Cellid
			FROM Dog;
        END

      IF @StatementType = 'Update'
        BEGIN
          UPDATE Dog SET 
				  ChipNumber = @ChipNumber, 
				  [Name] = @Name, 
				  DateOfBirth = @DateOfBirth, 
				  Gender = @Gender, 
				  EntrandeDate = @EntrandeDate, 
				  IsAdoptable = @IsAdoptable, 
				  [Size] = @Size, 
				  Adopted = @Adopted, 
				  IsReturned = @IsReturned, 
				  Cellid = @Cellid, 
				  Deleted = @Deleted 
				WHERE
				  NumberId = @NumberId;

        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           UPDATE Dog SET 
				  Deleted = 'true'
				WHERE
				  NumberId = @NumberId;

        END
  END
GO


Drop procedure if exists AdoptersTableIUD
GO
create PROCEDURE AdoptersTableIUD 
					@PhoneNumber char(10) , 
					  @Email varchar(40), 
					  @FirstName nvarchar(12), 
					  @LastName nvarchar(12), 
					  @DateOfBirth date, 
					  @HouseMembers nvarchar(20), 
					  @DogsPlace nvarchar(20), 
					  @AdditionalPets nvarchar(20), 
					  @Experience nvarchar(20), 
					  @Note nvarchar(2048), 
					  @Addressid int,
					@Deleted bit,
					@StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
			INSERT INTO Optional_adopter
					  (PhoneNumber, 
					  Email, 
					  FirstName, 
					  LastName, 
					  DateOfBirth, 
					  HouseMembers, 
					  DogsPlace, 
					  AdditionalPets, 
					  Experience, 
					  Note, 
					  Addressid 
					  ) 
					VALUES 
					  (@PhoneNumber, 
					  @Email, 
					  @FirstName, 
					  @LastName, 
					  @DateOfBirth, 
					  @HouseMembers, 
					  @DogsPlace, 
					  @AdditionalPets, 
					  @Experience, 
					  @Note, 
					  @Addressid 
					  );


				SELECT SCOPE_IDENTITY() 
			
        END

      IF @StatementType = 'Select'
        BEGIN
          SELECT PhoneNumber, Email, FirstName, LastName, DateOfBirth, HouseMembers, DogsPlace, AdditionalPets, Experience, Note,ua.Id, ua.HouseNumber, ua.StreetName,c.CityName, c.Region
		FROM Optional_adopter oa inner join  UserAddress ua on oa.Addressid = ua.id inner join City c on ua.Cityid = c.Id;

        END

      IF @StatementType = 'Update'
        BEGIN
        UPDATE Optional_adopter SET 
						  Email = @Email, 
						  FirstName = @FirstName, 
						  LastName = @LastName, 
						  DateOfBirth = @DateOfBirth, 
						  HouseMembers = @HouseMembers, 
						  DogsPlace = @DogsPlace, 
						  AdditionalPets = @AdditionalPets, 
						  Experience = @Experience, 
						  Note = @Note, 
						  Addressid = @Addressid, 
						  Deleted = @Deleted 
						WHERE
						  PhoneNumber = @PhoneNumber;


        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           UPDATE Optional_adopter SET 
				  Deleted = 'true'
				WHERE
				  PhoneNumber = @PhoneNumber;

        END
  END
GO


Drop procedure if exists CellsTableIUD
GO
create PROCEDURE CellsTableIUD 
					 @id int,
					@ShelterNumber int, 
					 @Number int, 
					  @capacity int, 
					@StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
			INSERT INTO Cell
					  (
					  ShelterNumber, 
					  Number, 
					  capacity) 
					VALUES 
					  (
						@ShelterNumber, 
						@Number, 
						@capacity);

				SELECT SCOPE_IDENTITY() 
			
        END

      IF @StatementType = 'Select'
        BEGIN
          SELECT PhoneNumber, Email, FirstName, LastName, DateOfBirth, HouseMembers, DogsPlace, AdditionalPets, Experience, Note, ua.HouseNumber, ua.StreetName  
		FROM Optional_adopter oa inner join  UserAddress ua on oa.Addressid = ua.id inner join City c on ua.Cityid = c.Id;

        END

      IF @StatementType = 'Update'
        BEGIN
        UPDATE Cell SET 
				  ShelterNumber = @ShelterNumber, 
				  Number = @Number, 
				  capacity = @capacity 
				WHERE
				  id = @id;


        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           DELETE FROM Cell 
				WHERE id = @id;;

        END
  END
GO


Drop procedure if exists VolunteerTableIUD
GO
create PROCEDURE VolunteerTableIUD 
					@PhoneNumber char(10), 
					  @Email varchar(40), 
					  @FirstName nvarchar(12), 
					  @LastName nvarchar(12), 
					@StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
			INSERT INTO Volunteer
					  (PhoneNumber, 
					  Email, 
					  FirstName, 
					  LastName) 
					VALUES 
					  (@PhoneNumber, 
					  @Email, 
					  @FirstName, 
					  @LastName);


				SELECT @PhoneNumber 
		
        END

      IF @StatementType = 'Select'
        BEGIN
         SELECT PhoneNumber, Email, FirstName, LastName 
		FROM Volunteer
		where Deleted = 'false';

        END

		 IF @StatementType = 'SelectOne'
        BEGIN
         SELECT PhoneNumber, Email, FirstName, LastName 
		FROM Volunteer
		where Deleted = 'false' and PhoneNumber=@PhoneNumber;

        END
      IF @StatementType = 'Update'
        BEGIN
        UPDATE Volunteer SET 
				  Email = @Email, 
				  FirstName = @FirstName, 
				  LastName = @LastName 
				WHERE
				  PhoneNumber = @PhoneNumber;


        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           UPDATE Volunteer SET 
				  Deleted = 'true'
				WHERE
				  PhoneNumber = @PhoneNumber;

        END
  END
GO


Drop procedure if exists AdminTableIUD
GO
create PROCEDURE AdminTableIUD 
					@PhoneNumber char(10), 
					  @Email varchar(40), 
					  @FirstName nvarchar(12), 
					  @LastName nvarchar(12), 
					  @UserName nvarchar(12),
					  @Password varchar(16),
					@StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
		INSERT INTO ShelterAdmin
					  (PhoneNumber, 
					  UserName, 
					  [Password], 
					  Email, 
					  FirstName, 
					  LastName 
					  ) 
					VALUES 
					  (@PhoneNumber, 
					  @UserName, 
					  @Password, 
					  @Email, 
					  @FirstName, 
					  @LastName 
					  );



				SELECT @PhoneNumber 
		
        END

      IF @StatementType = 'Select'
        BEGIN
			SELECT PhoneNumber, UserName, [Password], Email, FirstName, LastName 
			FROM ShelterAdmin
			where Deleted = 'false';

        END

		 IF @StatementType = 'SelectOne'
        BEGIN
              SELECT PhoneNumber, UserName, [Password], Email, FirstName, LastName 
				FROM ShelterAdmin
				where Deleted = 'false' and PhoneNumber=@PhoneNumber;

        END
      IF @StatementType = 'Update'
        BEGIN
        UPDATE ShelterAdmin SET 
				  UserName = @UserName, 
				  [Password] = @Password, 
				  Email =@Email, 
				  FirstName = @FirstName, 
				  LastName = @LastName 
				WHERE
				  PhoneNumber = @PhoneNumber;


        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           UPDATE ShelterAdmin SET 
				  Deleted = 'true'
				WHERE
				  PhoneNumber = @PhoneNumber;

        END
  END
GO



Drop procedure if exists ShelterTableIUD
GO
create PROCEDURE ShelterTableIUD 
					@ShelterNumber int, 
					  @AdminPhoneNumber char(10), 
					  @FacebookUserName nvarchar(30), 
					  @FacebookPassword nvarchar(20), 
					  @InstegramUserName nvarchar(30), 
					  @InstegramPassword nvarchar(20), 
					  @TimeToReport time, 
					  @Name nvarchar(12), 
					  @PhotoUrl nvarchar(1000), 
					  @AddressId int, 
					  @Deleted bit,
					@StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
		INSERT INTO Shelter
					  (ShelterNumber, 
					  AdminPhoneNumber, 
					  FacebookUserName, 
					  FacebookPassword, 
					  InstegramUserName, 
					  InstegramPassword, 
					  TimeToReport, 
					  [Name], 
					  PhotoUrl, 
					  AddressId, 
					  Deleted) 
					VALUES 
					  (@ShelterNumber, 
					  @AdminPhoneNumber, 
					  @FacebookUserName, 
					  @FacebookPassword, 
					  @InstegramUserName, 
					  @InstegramPassword, 
					  @TimeToReport, 
					  @Name, 
					  @PhotoUrl, 
					  @AddressId, 
					  @Deleted);


				SELECT SCOPE_IDENTITY() 
		
        END

      IF @StatementType = 'Select'
        BEGIN
			SELECT s.ShelterNumber, s.AdminPhoneNumber, s.FacebookUserName, s.FacebookPassword, s.InstegramUserName, s.InstegramPassword, s.TimeToReport, s.[Name], s.PhotoUrl, (ua.StreetName + ' '+cast(ua.HouseNumber as varchar(5)) +', ' +c.CityName) as [Address] 
			  FROM Shelter s inner join UserAddress ua on s.AddressId=ua.Id inner join City c on ua.Cityid = c.Id
			  where Deleted = 'false';

        END

		 IF @StatementType = 'SelectOne'
        BEGIN
              		SELECT s.ShelterNumber, s.AdminPhoneNumber, s.FacebookUserName, s.FacebookPassword, s.InstegramUserName, s.InstegramPassword, s.TimeToReport, s.[Name], s.PhotoUrl, (ua.StreetName + ' '+cast(ua.HouseNumber as varchar(5)) +', ' +c.CityName) as [Address] 
			  FROM Shelter s inner join UserAddress ua on s.AddressId=ua.Id inner join City c on ua.Cityid = c.Id
			  where Deleted = 'false' and s.ShelterNumber = @ShelterNumber;

        END
      IF @StatementType = 'Update'
        BEGIN
       UPDATE Shelter SET 
			  AdminPhoneNumber = @AdminPhoneNumber, 
			  FacebookUserName = @FacebookUserName, 
			  FacebookPassword = @FacebookPassword, 
			  InstegramUserName = @InstegramUserName, 
			  InstegramPassword = @InstegramPassword, 
			  TimeToReport = @TimeToReport, 
			  [Name] = @Name, 
			  PhotoUrl = @PhotoUrl, 
			  AddressId = @AddressId 
			WHERE
			  ShelterNumber = @ShelterNumber;


        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           UPDATE Shelter SET 
				  Deleted = 'true'
				WHERE
				  ShelterNumber = @ShelterNumber;

        END
  END
GO


