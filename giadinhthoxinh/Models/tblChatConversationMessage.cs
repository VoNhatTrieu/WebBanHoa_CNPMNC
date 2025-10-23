using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace giadinhthoxinh.Models
{
    public partial class tblChatConversationMessage
    {
        public int PK_iMessageID { get; set; }
        public int FK_iConversationID { get; set; }
        public string senderType { get; set; }  // 'customer' hoặc 'admin'
        public Nullable<int> FK_iSenderAccountID { get; set; }
        public string sSenderName { get; set; }
        public string sMessage { get; set; }
        public DateTime dSentAt { get; set; }
        public bool bIsRead { get; set; }

        public virtual tblChatConversation tblChatConversation { get; set; }
        
        // BỎ QUA navigation property để tránh EF tự động phát hiện tblUser
        [NotMapped]
        public virtual tblUser tblUser { get; set; }
    }
}
