-- ===============================================
-- Script tạo bảng cho chức năng Chat trực tiếp
-- Khách hàng - Admin
-- ===============================================

-- 1. Bảng lưu các cuộc trò chuyện (Conversation/Room)
CREATE TABLE tblChatConversation (
    PK_iConversationID INT IDENTITY(1,1) PRIMARY KEY,
    FK_iCustomerAccountID INT NULL,  -- NULL nếu khách chưa đăng nhập
    sCustomerEmail NVARCHAR(255) NOT NULL,  -- Email để phân biệt khách
    sCustomerName NVARCHAR(255) NULL,  -- Tên khách hàng (nếu có)
    FK_iAdminAccountID INT NULL,  -- Admin đang xử lý (NULL nếu chưa có admin)
    sAdminName NVARCHAR(255) NULL,  -- Tên admin đang xử lý
    sStatus NVARCHAR(50) DEFAULT N''Chờ admin'',  -- ''Chờ admin'', ''Đang chat'', ''Đã kết thúc''
    dCreatedAt DATETIME DEFAULT GETDATE(),
    dUpdatedAt DATETIME DEFAULT GETDATE(),
    dClosedAt DATETIME NULL,  -- Thời gian đóng chat
    FOREIGN KEY (FK_iCustomerAccountID) REFERENCES tblUser(PK_iAccountID) ON DELETE SET NULL,
    FOREIGN KEY (FK_iAdminAccountID) REFERENCES tblUser(PK_iAccountID) ON DELETE SET NULL
);

-- 2. Bảng lưu tin nhắn chi tiết
CREATE TABLE tblChatConversationMessage (
    PK_iMessageID INT IDENTITY(1,1) PRIMARY KEY,
    FK_iConversationID INT NOT NULL,
    senderType NVARCHAR(20) NOT NULL,  -- ''customer'' hoặc ''admin''
    FK_iSenderAccountID INT NULL,  -- ID tài khoản người gửi (NULL nếu khách chưa đăng nhập)
    sSenderName NVARCHAR(255) NOT NULL,  -- Tên người gửi
    sMessage NVARCHAR(MAX) NOT NULL,  -- Nội dung tin nhắn
    dSentAt DATETIME DEFAULT GETDATE(),
    bIsRead BIT DEFAULT 0,  -- Đã đọc chưa
    FOREIGN KEY (FK_iConversationID) REFERENCES tblChatConversation(PK_iConversationID) ON DELETE CASCADE,
    FOREIGN KEY (FK_iSenderAccountID) REFERENCES tblUser(PK_iAccountID) ON DELETE SET NULL
);

-- 3. Index để tăng hiệu suất query
CREATE INDEX idx_conversation_customer ON tblChatConversation(FK_iCustomerAccountID);
CREATE INDEX idx_conversation_email ON tblChatConversation(sCustomerEmail);
CREATE INDEX idx_conversation_status ON tblChatConversation(sStatus);
CREATE INDEX idx_message_conversation ON tblChatConversationMessage(FK_iConversationID);
CREATE INDEX idx_message_sentAt ON tblChatConversationMessage(dSentAt);

-- 4. Thêm trigger để tự động cập nhật thời gian
GO
CREATE TRIGGER tr_UpdateConversationTime
ON tblChatConversationMessage
AFTER INSERT
AS
BEGIN
    UPDATE tblChatConversation
    SET dUpdatedAt = GETDATE()
    WHERE PK_iConversationID IN (SELECT FK_iConversationID FROM inserted);
END;
GO

PRINT ''Đã tạo thành công bảng tblChatConversation và tblChatConversationMessage!'';
PRINT ''Đã tạo index và trigger!'';
