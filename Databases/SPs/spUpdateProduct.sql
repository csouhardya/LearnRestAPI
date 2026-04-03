CREATE OR ALTER PROCEDURE spUpdateProduct
@guid UNIQUEIDENTIFIER,
@name NVARCHAR(MAX),
@sku NVARCHAR(50),
@currency NVARCHAR(5),
@amount DECIMAL
AS
BEGIN
	UPDATE dbo.products SET name = @name, sku = @sku, currency = @currency, amount = @amount
	WHERE guid = @guid
END