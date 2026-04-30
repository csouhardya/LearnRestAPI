CREATE TYPE InventoryTVP AS TABLE
(
	product_guid UNIQUEIDENTIFIER NOT NULL UNIQUE,
	quantity_to_subtract INT NULL,
	absolute_quantity INT NULL
);