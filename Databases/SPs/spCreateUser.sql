CREATE OR ALTER PROCEDURE spCreateUser
@guid UNIQUEIDENTIFIER,
@first_name NVARCHAR(100),
@last_name NVARCHAR(100),
@date_of_birth DATE,
@email NVARCHAR(255),
@username NVARCHAR(30),
@password NVARCHAR(255),
@role INT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM dbo.Users u WHERE u.email = @email)
	BEGIN
		THROW 50001,'email already exists',0;
	END

	IF EXISTS (SELECT 1 FROM dbo.Users u WHERE u.username = @username)
	BEGIN
		THROW 50002,'username already exists',0;
	END

	INSERT INTO dbo.Users (guid, first_name, last_name, date_of_birth, email, username, password, role)
	VALUES(@guid, @first_name, @last_name, @date_of_birth, @email, @username, @password, @role)
END
