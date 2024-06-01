USE [igroup154_test1]
GO

Drop procedure if exists LoginAdmin
GO
create PROCEDURE LoginAdmin 
				@PhoneNumber char(10),
				@Password varchar(20)

AS
  BEGIN

    SELECT s.ShelterNumber
	from ShelterAdmin sa inner join Shelter s on sa.PhoneNumber=s.AdminPhoneNumber
	where sa.Password = @Password and sa.PhoneNumber=@PhoneNumber

  END



Drop procedure if exists CityTableIUD

CREATE PROCEDURE CityTableIUD 
AS
BEGIN

	SELECT CityName FROM City
end 

GO
create PROCEDURE CityTableIUD  @CityName NVARCHAR(12), @Region nvarchar(30),@StatementType varchar(10)
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
            SELECT CityName, Region
			FROM City;
        END

      IF @StatementType = 'Update'
        BEGIN
            UPDATE City SET 
			  Region = @Region 
			WHERE
			   CityName = @CityName;
        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           DELETE FROM City 
		 where CityName = @CityName;

        END
  END
GO

Drop procedure if exists UserAddressTableIUD
GO
create PROCEDURE UserAddressTableIUD 
				@Id int, 
				@HouseNumber int, 
				@StreetName nvarchar(12), 
				 @CityName NVARCHAR(12), 
				 @Region nvarchar(30),
				@StatementType varchar(10)
AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
				INSERT INTO UserAddress
				  (HouseNumber, 
				  StreetName, 
				  City) 
				VALUES 
				   ( @HouseNumber, 
					  @StreetName, 
					  @CityName) ;
				SELECT SCOPE_IDENTITY() 
			
        END

      IF @StatementType = 'Select'
        BEGIN
            SELECT ua.Id, ua.HouseNumber, ua.StreetName, c.CityName ,c.Region
			FROM UserAddress ua inner join City c on ua.City = c.CityName
		
        END

      IF @StatementType = 'Update'
        BEGIN

           UPDATE UserAddress SET 
					HouseNumber = @HouseNumber, 
					StreetName = @StreetName, 
					City = @CityName 
				WHERE
					Id = @Id;

        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
          DELETE FROM UserAddress 
		 WHERE Id = @Id;

        END
  END

Drop procedure if exists BreedTableIUD
GO


Drop procedure if exists BreedTableIUD
GO
create PROCEDURE BreedTableIUD 
				@Breed  nvarchar(12), 
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
            SELECT Breed
			From Breed

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
				  @NumberId int, 
				  @Name nvarchar(20), 
				  @DateOfBirth date, 
				  @Gender nvarchar(4), 
				  @EntranceDate date, 
				  @IsAdoptable bit, 
				  @Size varchar(20), 
				  @Adopted bit, 
				  @IsReturned bit, 
				  @Cellid int, 
				  @shelter int,
				  @StatementType varchar(20)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
			INSERT INTO Dog
				  (ChipNumber, 
				  [Name], 
				  DateOfBirth, 
				  Gender, 
				  EntranceDate, 
				  IsAdoptable, 
				  [Size], 
				  Adopted, 
				  IsReturned, 
				  Cellid
				  ) 
				VALUES 
				  (@ChipNumber, 
				  @Name, 
				  @DateOfBirth, 
				  @Gender, 
				  @EntranceDate, 
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
            SELECT ChipNumber, NumberId, [Name], DateOfBirth, Gender, EntranceDate, IsAdoptable, [Size], Adopted, IsReturned, Cellid,profileImg
			FROM Dog;
        END

		IF @StatementType = 'SelectOne'
        BEGIN
            SELECT ChipNumber, NumberId, [Name], DateOfBirth, Gender, EntranceDate, IsAdoptable, [Size], Adopted, IsReturned, Cellid,profileImg
			FROM Dog
			where NumberId=3;
        END

		IF @StatementType = 'SelectByShelter'
        BEGIN
		  SELECT d.*,c.Number
			FROM Dog d join Cell c on d.Cellid=c.id
			where c.ShelterNumber = @shelter and d.Adopted = 'false'
        END

      IF @StatementType = 'Update'
        BEGIN
          UPDATE Dog SET 
				  ChipNumber = @ChipNumber, 
				  [Name] = @Name, 
				  DateOfBirth = @DateOfBirth, 
				  Gender = @Gender, 
				  EntranceDate = @EntranceDate, 
				  IsAdoptable = @IsAdoptable, 
				  [Size] = @Size, 
				  Adopted = @Adopted, 
				  IsReturned = @IsReturned, 
				  Cellid = @Cellid
				WHERE
				  NumberId = @NumberId;

        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
           UPDATE Dog SET 
				  Adopted = 'true'
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
					  @HouseMembers nvarchar(20), 
					  @DateOfBirth date = GETDATE, 
					  @DogsPlace nvarchar(20), 
					  @AdditionalPets nvarchar(20), 
					  @Experience nvarchar(20), 
					  @Note nvarchar(2048), 
					  @Addressid int,
					@Deleted bit = false,
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


				SELECT @PhoneNumber
			
        END

      IF @StatementType = 'Select'
        BEGIN
          SELECT PhoneNumber, Email, FirstName, LastName, DateOfBirth, HouseMembers, DogsPlace, AdditionalPets, Experience, Note,ua.Id, ua.HouseNumber, ua.StreetName,c.CityName, c.Region
		FROM Optional_adopter oa inner join  UserAddress ua on oa.Addressid = ua.id inner join City c on ua.City = c.CityName
		where oa.Deleted='false';

        END
      IF @StatementType = 'SelectOne'
        BEGIN
          SELECT PhoneNumber, Email, FirstName, LastName, DateOfBirth, HouseMembers, DogsPlace, AdditionalPets, Experience, Note,ua.Id, ua.HouseNumber, ua.StreetName,c.CityName, c.Region
		FROM Optional_adopter oa inner join  UserAddress ua on oa.Addressid = ua.id inner join City c on ua.City = c.CityName
		where phoneNumber=@PhoneNumber and oa.Deleted='false';

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
					@StatementType varchar(20)

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

				SELECT SCOPE_IDENTITY(); 
			
        END
	IF @StatementType = 'SelectFromShelter'
        BEGIN
			SELECT Id, ShelterNumber, Number,  capacity
			FROM Cell
			where ShelterNumber=@ShelterNumber ;

        END
    IF @StatementType = 'Select'
        BEGIN
          SELECT Id, ShelterNumber, Number,  capacity
		FROM Cell;

        END
	IF @StatementType = 'SelectOne'
        BEGIN
          SELECT Id, ShelterNumber, Number,  capacity
		FROM Cell
		where id=@id;

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
				WHERE id = 2;

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
					  @InstagramUserName nvarchar(30), 
					  @InstagramPassword nvarchar(20), 
					  @TimeToReport time, 
					  @Name nvarchar(12), 
					  @PhotoUrl nvarchar(1000), 
					  @AddressId int, 
					  @Deleted bit='false',
					@StatementType varchar(10)

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
		INSERT INTO Shelter
					  (
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
					  ( 
					  @AdminPhoneNumber, 
					  @FacebookUserName, 
					  @FacebookPassword, 
					  @InstagramUserName, 
					  @InstagramPassword, 
					  @TimeToReport, 
					  @Name, 
					  @PhotoUrl, 
					  @AddressId, 
					  @Deleted);


				SELECT SCOPE_IDENTITY() 
		
        END

      IF @StatementType = 'Select'
        BEGIN
              		SELECT s.ShelterNumber, s.AdminPhoneNumber, s.FacebookUserName, s.FacebookPassword, s.InstegramUserName, s.InstegramPassword, s.TimeToReport, s.[Name], s.PhotoUrl, ua.Id as AddressId,ua.StreetName,ua.HouseNumber ,ua.City,c.Region
			  FROM Shelter s inner join UserAddress ua on s.AddressId=ua.Id inner join City c on ua.City = c.CityName
			  where Deleted = 'false';

        END

		 IF @StatementType = 'SelectOne'
        BEGIN
              		SELECT s.ShelterNumber, s.AdminPhoneNumber, s.FacebookUserName, s.FacebookPassword, s.InstegramUserName, s.InstegramPassword, s.TimeToReport, s.[Name], s.PhotoUrl, ua.Id as AddressId,ua.StreetName,ua.HouseNumber ,ua.City,c.Region
			  FROM Shelter s inner join UserAddress ua on s.AddressId=ua.Id inner join City c on ua.City = c.CityName
			  where Deleted = 'false' and s.ShelterNumber = @ShelterNumber;

        END
      IF @StatementType = 'Update'
        BEGIN
       UPDATE Shelter SET 
			  AdminPhoneNumber = @AdminPhoneNumber, 
			  FacebookUserName = @FacebookUserName, 
			  FacebookPassword = @FacebookPassword, 
			  InstegramUserName = @InstagramUserName, 
			  InstegramPassword = @InstagramPassword, 
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


Drop procedure if exists AdoptionRequestTableIUD
GO
create PROCEDURE AdoptionRequestTableIUD 
					@RequestID int = null ,
					@Optional_adopterPhoneNumber char(10) = null,
					@SentDate date,
					@Status nvarchar(5) = null,
					@DogNumberId int = null,
					@Deleted bit = null,
					@StatementType varchar(10) =null

AS
  BEGIN
      IF @StatementType = 'Insert'
        BEGIN
		INSERT INTO Adoption_request
						  ( 
						  Optional_adopterPhoneNumber, 
						  SentDate, 
						  [Status],
						  DogNumberId 
						  ) 
						VALUES 
						  ( 
						  @Optional_adopterPhoneNumber, 
						  @SentDate, 
						  @Status, 
						  @DogNumberId
						  );



				SELECT SCOPE_IDENTITY() 
		
        END

      IF @StatementType = 'Select'
        BEGIN
			SELECT RequestID, Optional_adopterPhoneNumber, SentDate, Status, DogNumberId, Deleted 
			FROM Adoption_request
			where Deleted = 'false';

        END
      IF @StatementType = 'Update'
        BEGIN
        UPDATE Adoption_request SET 
				  Optional_adopterPhoneNumber = @Optional_adopterPhoneNumber , 
				  SentDate = @SentDate, 
				  Status = @Status,
				  DogNumberId = @DogNumberId
				WHERE
				  RequestID = @RequestID;


        END
      ELSE
	  IF @StatementType = 'Delete'
        BEGIN
		UPDATE Adoption_request SET
				Deleted ='true'
		WHERE RequestID = @RequestID;


        END
  END
GO

Drop procedure if exists ColorTableIUD
GO
create PROCEDURE ColorTableIUD 
AS
  BEGIN
      SELECT ColorName 
			FROM Color
		
        END
		Go

Drop procedure if exists CharacteristicsTableIUD
GO
create PROCEDURE CharacteristicsTableIUD
AS
  BEGIN
		IF @StatementType = 'Insert'
        BEGIN
		INSERT INTO Characteristics_Dog
						  ( 
						  attribute, 
						  DogNumberId 
						  ) 
						VALUES 
						  ( 
						  @attribute, 
						  @DogNumberId
						  );
		
        END

		IF @StatementType = 'Select'
		BEGIN
		SELECT ColorName 
			FROM Color
		END
		
        END
		Go
