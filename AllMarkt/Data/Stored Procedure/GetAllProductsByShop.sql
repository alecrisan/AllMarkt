USE [AllMarkt]
GO
CREATE OR ALTER PROCEDURE [dbo].[GetAllProductsByShop]
	@shopId INTEGER
AS
BEGIN
	SELECT  
		P.Id,
		P.[Name],
		P.[Description],
		P.Price,
		P.ImageURI,
		P.[State],
		P.ProductCategoryId,
		PC.[Name] AS ProductCategoryName,
		COALESCE(AVG(CAST(PCOM.Rating AS FLOAT)), 0) AS [AverageRating]
	FROM
	Products AS P 
	LEFT JOIN 
	ProductCategories AS PC
	ON P.ProductCategoryId = PC.Id
	LEFT JOIN
	Shops AS S
	ON PC.ShopId = S.Id
	LEFT JOIN
	ProductComments AS PCOM
	ON PCOM.ProductId = P.Id
	WHERE S.Id = @shopId
	GROUP BY P.Id,P.[Name],P.[Description],P.Price,P.ImageURI,P.[State],P.ProductCategoryId,PC.[Name]
END