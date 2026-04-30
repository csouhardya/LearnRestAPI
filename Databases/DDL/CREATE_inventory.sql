CREATE TABLE dbo.Inventory
(
	product_guid UNIQUEIDENTIFIER UNIQUE NOT NULL,
	quantity INT NOT NULL

	FOREIGN KEY(product_guid) REFERENCES products(guid)
);
