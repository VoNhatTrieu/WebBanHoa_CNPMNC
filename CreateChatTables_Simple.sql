-- Xóa bảng cũ nếu tồn tại (chỉ để test)
IF OBJECT_ID('tblChatConversationMessage', 'U') IS NOT NULL
    DROP TABLE tblChatConversationMessage;
IF OBJECT_ID('tblChatConversation', 'U') IS NOT NULL
    DROP TABLE tblChatConversation;
GO

-- 1. Tạo bảng Conversation
CREATE TABLE tblChatConversation (
    PK_iConversationID INT IDENTITY(1,1) PRIMARY KEY,
    FK_iCustomerAccountID INT NULL,
    sCustomerEmail NVARCHAR(255) NOT NULL,
    sCustomerName NVARCHAR(255) NULL,
    FK_iAdminAccountID INT NULL,
    sAdminName NVARCHAR(255) NULL,
    sStatus NVARCHAR(50) DEFAULT N'Chờ admin',
    dCreatedAt DATETIME DEFAULT GETDATE(),
    dUpdatedAt DATETIME DEFAULT GETDATE(),
    dClosedAt DATETIME NULL,
    FOREIGN KEY (FK_iCustomerAccountID) REFERENCES tblUser(PK_iAccountID) ON DELETE SET NULL,
    FOREIGN KEY (FK_iAdminAccountID) REFERENCES tblUser(PK_iAccountID) ON DELETE SET NULL
);

-- 2. Tạo bảng Message
CREATE TABLE tblChatConversationMessage (
    PK_iMessageID INT IDENTITY(1,1) PRIMARY KEY,
    FK_iConversationID INT NOT NULL,
    senderType NVARCHAR(20) NOT NULL,
    FK_iSenderAccountID INT NULL,
    sSenderName NVARCHAR(255) NOT NULL,
    sMessage NVARCHAR(MAX) NOT NULL,
    dSentAt DATETIME DEFAULT GETDATE(),
    bIsRead BIT DEFAULT 0,
    FOREIGN KEY (FK_iConversationID) REFERENCES tblChatConversation(PK_iConversationID) ON DELETE CASCADE,
    FOREIGN KEY (FK_iSenderAccountID) REFERENCES tblUser(PK_iAccountID) ON DELETE SET NULL
);

-- 3. Tạo Index
CREATE INDEX idx_conversation_customer ON tblChatConversation(FK_iCustomerAccountID);
CREATE INDEX idx_conversation_email ON tblChatConversation(sCustomerEmail);
CREATE INDEX idx_conversation_status ON tblChatConversation(sStatus);
CREATE INDEX idx_message_conversation ON tblChatConversationMessage(FK_iConversationID);
CREATE INDEX idx_message_sentAt ON tblChatConversationMessage(dSentAt);

PRINT 'Đã tạo thành công các bảng chat!';
