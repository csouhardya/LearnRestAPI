CREATE OR ALTER PROCEDURE spAddInventory
	@product_guid UNIQUEIDENTIFIER,
	@quantity INT
AS
BEGIN
	INSERT INTO dbo.Inventory(product_guid, quantity) VALUES (@product_guid, @quantity)
END
GO