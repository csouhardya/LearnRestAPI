CREATE OR ALTER PROCEDURE spGetUserByUsername
@username NVARCHAR(30)
AS
BEGIN
	IF @username IS NULL
	BEGIN
		THROW 500001, 'username cannot be null', 1;
	END

	SELECT guid,
	first_name as firstName,
	last_name as lastName,
	date_of_birth as dateOfBirth,
	email,
	username,
	password,
	role
	FROM dbo.Users u 
	WHERE 
	u.Username = @username
END

