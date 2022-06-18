CREATE DATABASE QuanLyQuanNet
GO

USE QuanLyQuanNet
GO

-- Order
-- Computer
-- OrderCategory
-- Account
-- Bill
-- BillInfo

CREATE TABLE ComputerOrder
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Máy chưa có tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'	-- Trống || Có người
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY,	
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Tthien',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO
CREATE TABLE OrderCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Orderr
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.OrderCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idComputer INT NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idComputer) REFERENCES dbo.ComputerOrder(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idOrder INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idOrder) REFERENCES dbo.Orderr(id)
)
GO

INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'Thien2605' , -- UserName - nvarchar(100)
          N'ThienDeJong' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          1  -- Type - int
        )
INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'staff' , -- UserName - nvarchar(100)
          N'staff' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )
GO

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS 
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'Thien2605' -- nvarchar(100) --USP - User stored procedure

-- SELECT * FROM dbo.Account WHERE UserName = N'Thien2605' AND PassWord = N'1'

Go

CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO
-- Thêm máy
DECLARE @i INT = 0 

WHILE @i <= 30
BEGIN
	INSERT dbo.ComputerOrder ( name)VALUES  ( N'Máy ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END

Go

CREATE PROC USP_GetComputerList
AS SELECT * FROM dbo.ComputerOrder
GO

UPDATE dbo.ComputerOrder SET STATUS = N'Có người' WHERE id = 14

EXEC dbo.USP_GetComputerList

GO
-- thêm category
INSERT dbo.OrderCategory
        ( name )
VALUES  ( N'Đồ ăn nước'  -- name - nvarchar(100)
          )
INSERT dbo.OrderCategory
        ( name )
VALUES  ( N'Đồ ăn khô' )
INSERT dbo.OrderCategory
        ( name )
VALUES  ( N'Đồ ăn nhanh' )
INSERT dbo.OrderCategory
        ( name )
VALUES  ( N'Giờ chơi' )
INSERT dbo.OrderCategory
        ( name )
VALUES  ( N'Nước' )

-- thêm món ăn


INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Mì Trứng', 1, 20000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Phở', 1, 30000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Hủ tiếu', 1, 25000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Cơm chiên dương châu', 2, 25000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Cơm chiên hải sản', 2, 35000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Mì xào hải sản', -- name - nvarchar(100)
          2, -- idCategory - int
          30000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Tokbokki', 2, 25000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Mì xào bò', 2, 25000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Xúc xích Đức', 3, 12000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Khoai tây chiên', 3, 15000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Cá viên chiên', 3, 10000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Bánh mì', 3, 15000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'1 Giờ', 4, 10000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Sting', 5, 15000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Pepsi', 5, 15000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'7 UP', 5, 15000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Cafe đá', 5, 15000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Cafe sữa', 5, 18000)
INSERT dbo.Orderr
        ( name, idCategory, price )
VALUES  ( N'Monster', 5, 50000)


-- Thêm Bill

INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idComputer ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          1 , -- idComputer - int
          0  -- status - int
        )
        
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idComputer ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          2, -- idComputer - int
          0  -- status - int
        )
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idComputer ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          GETDATE() , -- DateCheckOut - date
          3 , -- idComputer - int
          1  -- status - int
        )

-- thêm bill info
INSERT	dbo.BillInfo
        ( idBill, idOrder, count )
VALUES  ( 2, -- idBill - int
          1, -- idOrder - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idOrder, count )
VALUES  ( 3, -- idBill - int
          2, -- idOrder - int
          4  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idOrder, count )
VALUES  ( 1, -- idBill - int
          3, -- idOrder - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idOrder, count )
VALUES  ( 4, -- idBill - int
          4, -- idOrder - int
          1  -- count - int
          )     
          
GO

CREATE PROC USP_InsertBill
@idComputer INT
AS
BEGIN
	INSERT dbo.Bill
			(	DateCheckIn ,
				DateCheckOut ,
				idComputer ,
				status,
				discount
			)
	VALUES  (	GETDATE() , --Datecheckin
				GETDATE() , --Datecheckout
				@idComputer , --idTable
				0,  -- status -int
				0
			)
END
GO

CREATE PROC USP_InsertBillInfo
@idBill INT , @idOrder INT , @count INT
AS
BEGIN
	INSERT dbo.BillInfo
			( idBill , idOrder , count )
VALUES	(	@idBill , -- idBill - int
			@idOrder , -- idOrder - int
			@count  -- count - int

			)
END
GO

ALTER PROC USP_InsertBillInfo
@idBill INT , @idOrder INT , @count INT
AS
BEGIN
	
	DECLARE @isExitsBillInfo INT
	DECLARE @orderCount INT = 1
	SELECT @isExitsBillInfo = id, @orderCount = b.count
	FROM dbo.BillInfo AS b
	WHERE idBill = @idBill AND idOrder = @idOrder

	IF (@isExitsBillInfo > 0)
	BEGIN 
		DECLARE @newCount INT = @orderCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo SET count = @orderCount + @count WHERE idOrder = @idOrder
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idOrder = @idOrder
	END
	ELSE
	BEGIN
		INSERT dbo.BillInfo
		( idBill, idOrder, count )
		VALUES ( @idBill,
			@idOrder,
			@count
			)
	END
END
GO

DELETE dbo.BillInfo

DELETE dbo.Bill

CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idComputer INT
	
	SELECT @idComputer = idComputer FROM dbo.Bill WHERE id = @idBill AND status = 0

	DECLARE @count INT
	SELECT @count = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBill

	IF (@count > 0)
	BEGIN
	
		PRINT @idComputer
		PRINT @idBill
		PRINT @count
	
		UPDATE dbo.ComputerOrder SET status = N'Có người' WHERE id = @idComputer
	END

	ELSE
	BEGIN
	PRINT @idComputer
		PRINT @idBill
		PRINT @count
	UPDATE dbo.ComputerOrder SET status = N'Trống' WHERE id = @idComputer
	END
END
GO



CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idComputer INT
	
	SELECT @idComputer = idComputer FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count int = 0
	
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idComputer = @idComputer AND status = 0
	
	IF (@count = 0)
		UPDATE dbo.ComputerOrder SET status = N'Trống' WHERE id = @idComputer
END
GO

CREATE TABLE dbo.Bill
ADD discount INT

UPDATE dbo.Bill SET discount = 0
GO

CREATE PROC USP_SwitchComputer
@idComputer1 INT, @idComputer2 INT
AS 
BEGIN
	
	DECLARE @idFirstBill INT
	DECLARE @idSeconrdBill INT 

	DECLARE @isFirstComlEmty INT = 1
	DECLARE @isSecondComlEmty INT = 1

	SELECT @idSeconrdBill = id FROM dbo.Bill WHERE idComputer = @idComputer2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idComputer = @idComputer1 AND status = 0

	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'


	IF (@idFirstBill IS NULL)
	BEGIN
		PRINT '0000001'
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idComputer ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idComputer2 , -- idComputer - int
		          0  -- status - int
		        )
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE idComputer = @idComputer2 AND status = 0
		
	END

	SELECT @isFirstComlEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'
	
	IF (@idSeconrdBill IS NULL)
	BEGIN
		PRINT '0000002'
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idComputer ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idComputer2 , -- idTable - int
		          0  -- status - int
		        )
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE idComputer = @idComputer2 AND status = 0
		
	END
	
	SELECT @isSecondComlEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSeconrdBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'


	SELECT id INTO IDBillInfoComputer FROM dbo.BillInfo WHERE idBill = @idSeconrdBill

	UPDATE dbo.BillInfo SET idBill = @idSeconrdBill WHERE idBill = @idFirstBill

	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoComputer)

	DROP TABLE IDBillInfoComputer

	IF (@isFirstComlEmty = 0)
		UPDATE dbo.ComputerOrder SET status = N'Trống' WHERE id = @idComputer2
		
	IF (@isSecondComlEmty= 0)
		UPDATE dbo.ComputerOrder SET status = N'Trống' WHERE id = @idComputer1

END
GO

EXEC dbo.USP_SwitchComputer 
@idComputer1 = 1,
	@idComputer2 = 2

CREATE TABLE dbo.Bill ADD totalPrice FLOAT

DELETE dbo.BillInfo
DELETE dbo.Bill
GO

CREATE PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT t.name AS [Computer], b.totalPrice AS [Total Price], DateCheckIn AS [Date Checkin], DateCheckOut AS [Date Checkout], discount AS [Discount]
	FROM dbo.Bill AS b,dbo.ComputerOrder AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idComputer
END
GO

CREATE PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	
	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE USERName = @userName AND PassWord = @password
	
	IF (@isRightPass = 1)
	BEGIN
		IF (@newPassword = NULL OR @newPassword = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END		
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPassword WHERE UserName = @userName
	end
END
GO

CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS 
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = Deleted.idBill FROM Deleted
	
	DECLARE @idComputer INT
	SELECT @idComputer = idComputer FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count INT = 0
	
	SELECT @count = COUNT(*) FROM dbo.BillInfo AS bi, dbo.Bill AS b WHERE b.id = bi.idBill AND b.id = @idBill AND b.status = 0
	
	IF (@count = 0)
		UPDATE dbo.ComputerOrder SET status = N'Trống' WHERE id = @idComputer
END
GO
-- viết bằng tiếng Việt
CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END

GO

CREATE PROC USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
AS 
BEGIN
	DECLARE @pageRows INT = 10
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows
	
	;WITH BillShow AS( SELECT b.ID, t.name AS [Computer], b.totalPrice AS [Total Price], DateCheckIn AS [Date Checkin], DateCheckOut AS [Date Checkout], discount AS [Discount]
	FROM dbo.Bill AS b,dbo.ComputerOrder AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idComputer)
	
	SELECT TOP (@selectRows) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRows) id FROM BillShow)
END
GO

CREATE PROC USP_GetNumBillByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT COUNT(*)
	FROM dbo.Bill AS b,dbo.ComputerOrder AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idComputer
END
GO
--SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Orderr AS f WHERE bi.idBill = b.id AND bi.idOrder = f.id AND b.status = 0 AND b.idComputer = 3


--SELECT MAX(id) FROM dbo.Bill

--SELECT * FROM dbo.ComputerOrder
--SELECT * FROM dbo.Bill
--SELECT * FROM dbo.BillInfo
--SELECT * FROM dbo.OrderCategory
--SELECT * FROM dbo.BillInfo WHERE idBill = 5

--SELECT * FROM dbo.Orderr

--DELETE FROM ComputerOrder
--DELETE FROM Orderr --WHERE id = 20
--DELETE FROM OrderCategory

--DELETE dbo.BillInfo
--DELETE FROM IDBillInfoComputer


--DBCC CHECKIDENT('ComputerOrder', RESEED, 0)
--DBCC CHECKIDENT('Orderr', RESEED, 0)
--DBCC CHECKIDENT('OrderCategory', RESEED, 0)
--DBCC CHECKIDENT('Bill', RESEED, 0)
--DBCC CHECKIDENT('BillInfo', RESEED, 0)