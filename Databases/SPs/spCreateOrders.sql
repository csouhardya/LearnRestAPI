CREATE OR ALTER PROCEDURE spCreateOrders
@tempOrders OrderTVP READONLY
AS
BEGIN
	INSERT INTO dbo.Orders
    (
        guid,
        count,
        total_amount,
        created_date,
        order_status,
        tracking_id,
        email,
        username,
        user_guid,
        product_guid,
        product_name
    )
    SELECT
        t.guid,
        t.count,
        t.total_amount,
        GETDATE(),
        t.status,
        t.tracking_id,
        t.email_address,
        t.username,
        t.user_guid,
        t.product_guid,
        t.product_name
    FROM @tempOrders t;
END
	