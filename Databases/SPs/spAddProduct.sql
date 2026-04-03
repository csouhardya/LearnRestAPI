CREATE OR ALTER PROCEDURE spAddProduct
@guid UNIQUEIDENTIFIER,
@name NVARCHAR(MAX),
@sku NVARCHAR(50),
@currency NVARCHAR(5),
@amount DECIMAL
AS
BEGIN
	INSERT INTO dbo.products(guid, name, sku,currency, amount) VALUES (@guid, @name, @sku, @currency, @amount)
END