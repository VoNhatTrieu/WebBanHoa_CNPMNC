-- Thêm cột sStatus vào bảng tblCheckoutDetail
USE giadinhthoxinh;
GO

-- Kiểm tra xem cột đã tồn tại chưa
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblCheckoutDetail]') 
    AND name = 'sStatus'
)
BEGIN
    -- Thêm cột sStatus
    ALTER TABLE tblCheckoutDetail 
    ADD sStatus NVARCHAR(50) NULL DEFAULT N'Chờ xác nhận';
    
    PRINT 'Đã thêm cột sStatus vào bảng tblCheckoutDetail';
END
ELSE
BEGIN
    PRINT 'Cột sStatus đã tồn tại trong bảng tblCheckoutDetail';
END
GO

-- Cập nhật giá trị mặc định cho các dòng hiện tại
UPDATE tblCheckoutDetail 
SET sStatus = N'Chờ xác nhận' 
WHERE sStatus IS NULL;
GO

PRINT 'Hoàn tất migration!';
PRINT 'Tổng số dòng đã cập nhật: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
GO
