CREATE OR ALTER PROCEDURE spUpdateBulkInventory
	@tempInventory InventoryTVP READONLY
AS
BEGIN
	IF EXISTS(SELECT  1 FROM @tempInventory WHERE quantity_to_subtract IS NOT NULL) -- if updated from kafka upon placing order
	BEGIN 
		UPDATE i	
		SET 
			i.quantity =
				CASE
					WHEN i.quantity >= t.quantity_to_subtract
					THEN i.quantity - t.quantity_to_subtract
					ELSE 0
				END
		FROM dbo.Inventory i 
		INNER JOIN @tempInventory t
		ON i.product_guid = t.product_guid
	END

	ELSE IF EXISTS(SELECT  1 FROM @tempInventory WHERE absolute_quantity IS NOT NULL)-- if updated by admin to change inventory quantity
	BEGIN
		UPDATE i	
		SET 
			i.quantity = t.absolute_quantity
		FROM dbo.Inventory i 
		INNER JOIN @tempInventory t
		ON i.product_guid = t.product_guid	
	END
END
GO