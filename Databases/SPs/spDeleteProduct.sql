CREATE OR ALTER PROCEDURE spDeleteProduct
@guid UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM dbo.products
    WHERE guid = @guid;
END