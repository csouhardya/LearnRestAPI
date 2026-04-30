CREATE OR ALTER PROCEDURE spGetInventoryByProduct
	@product_guid UNIQUEIDENTIFIER
AS
BEGIN
	SELECT * FROM dbo.Inventories WHERE product_guid = @product_guid
END
GO