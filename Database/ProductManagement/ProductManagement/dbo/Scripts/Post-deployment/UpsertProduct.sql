SET NOCOUNT ON;

DECLARE @Date AS DATETIME;

SELECT @Date = DateAdd(MONTH,-3, GETDATE());
EXEC [dbo].[spProduct_Upsert] @Code = N'GDN-0011', @Name = N'Leaf Rake', @ReleaseDate =  @Date,  @Description = N'Leaf rake with 48-inch wooden.', @Price = 19.85, @StarRating = 4.5, @ImageUrl = N'./assets/leafrake.jfif';

SELECT @Date = DateAdd(MONTH,-18, GETDATE());
EXEC [dbo].[spProduct_Upsert] @Code = N'GDN-0023', @Name = N'Garden Cart', @ReleaseDate =  @Date,  @Description = N'15 gallon capacity rolling garden cart.', @Price = 35.25, @StarRating = 4.8, @ImageUrl = N'./assets/gardencart.jfif';

SELECT @Date = DateAdd(MONTH,-5, GETDATE());
EXEC [dbo].[spProduct_Upsert] @Code = N'TBX-0048', @Name = N'Hammer', @ReleaseDate =  @Date,  @Description = N'Curved claw steel hammer.', @Price = 50.6, @StarRating = 2.6, @ImageUrl = N'./assets/hammer.jfif';

SELECT @Date = DateAdd(MONTH,-3, GETDATE());
EXEC [dbo].[spProduct_Upsert] @Code = N'TBX-0022', @Name = N'Saw', @ReleaseDate =  @Date,  @Description = N'15-inch steel blade hand saw', @Price = 110, @StarRating = 5, @ImageUrl = N'./assets/saw.jfif';

SELECT @Date = DateAdd(MONTH,-1, GETDATE());
EXEC [dbo].[spProduct_Upsert] @Code = N'GMG-0042', @Name = N'Video Game Controller', @ReleaseDate =  @Date,  @Description = N'Standard two-button video game controller', @Price = 68.78, @StarRating = 1.5, @ImageUrl = N'./assets/gamecontroller.jfif';
