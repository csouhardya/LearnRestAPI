CREATE OR ALTER PROCEDURE spUpdateOrders
@tempOrders OrderTVP READONLY
AS
BEGIN
	UPDATE o
    SET
        o.order_status = t.status,
        o.tracking_id = t.tracking_id
    FROM dbo.Orders o
    INNER JOIN @tempOrders t
        ON o.guid = t.guid;

END