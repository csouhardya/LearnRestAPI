CREATE TYPE dbo.OrderTVP AS TABLE
(
    guid UNIQUEIDENTIFIER NOT NULL,
    count INT NOT NULL,
    total_amount DECIMAL(18,2) NOT NULL,
    product_guid UNIQUEIDENTIFIER NOT NULL,
    product_name NVARCHAR(MAX) NOT NULL,
    status INT NOT NULL,
    tracking_id NVARCHAR(100) NULL,
    user_guid UNIQUEIDENTIFIER NOT NULL,
    username NVARCHAR(30) NOT NULL,
    email_address NVARCHAR(255) NOT NULL
);