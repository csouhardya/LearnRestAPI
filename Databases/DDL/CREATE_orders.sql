CREATE TABLE dbo.Orders (
	id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	guid UNIQUEIDENTIFIER NOT NULL,
    count INT NOT NULL,
    total_amount DECIMAL(18,2) NOT NULL,
    created_date DATE NOT NULL,
    order_status INT NULL,
    tracking_id NVARCHAR(50) NULL,

	email NVARCHAR(255) NOT NULL,
	username NVARCHAR(30) NOT NULL,
    user_guid UNIQUEIDENTIFIER NOT NULL,

    product_guid UNIQUEIDENTIFIER NOT NULL,
    product_name NVARCHAR(MAX) NOT NULL,

    FOREIGN KEY(email) REFERENCES Users(email),
    FOREIGN KEY(username) REFERENCES Users(username),
    FOREIGN KEY(user_guid) REFERENCES Users(guid),
    FOREIGN KEY(product_guid) REFERENCES Products(guid)
)