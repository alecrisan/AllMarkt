USE [AllMarkt]
GO
/****** Object:  StoredProcedure [dbo].[GetProductsByCategoryIdWithAverageRating]    Script Date: 9/4/2019 3:29:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [dbo].[GetProductsByCategoryIdWithAverageRating]
	@productCategoryId INTEGER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		P.Id,
		P.[Name],
		P.[Description],
		P.Price,
		P.ImageURI,
		P.[State],
		P.ProductCategoryId,
		ProductCategories.[Name] AS ProductCategoryName,
		COALESCE(AVG(CAST(PC.Rating AS FLOAT)), 0) AS [AverageRating]
	FROM  Products AS P
	LEFT JOIN ProductComments AS PC
		ON P.Id = PC.ProductId
	LEFT JOIN ProductCategories 
		ON P.ProductCategoryId = ProductCategories.Id
	WHERE P.ProductCategoryId = @productCategoryId
	GROUP BY P.Id,P.[Name],P.[Description],P.Price,P.ImageURI,P.[State],P.ProductCategoryId, ProductCategories.[Name]
END