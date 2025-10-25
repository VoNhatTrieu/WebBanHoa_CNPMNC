using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace giadinhthoxinh.Models
{
    public partial class tblChatConversation
    {
        public tblChatConversation()
        {
            tblChatConversationMessages = new HashSet<tblChatConversationMessage>();
        }

        public int PK_iConversationID { get; set; }
        public Nullable<int> FK_iCustomerAccountID { get; set; }
        public string sCustomerEmail { get; set; }
        public string sCustomerName { get; set; }
        public Nullable<int> FK_iAdminAccountID { get; set; }
        public string sAdminName { get; set; }
        public string sStatus { get; set; }  // 'Đang chờ', 'Đang chat', 'Đã kết thúc'
        public DateTime dCreatedAt { get; set; }
        public DateTime dUpdatedAt { get; set; }
        public Nullable<DateTime> dClosedAt { get; set; }

        // BỎ QUA navigation properties để tránh EF tự động phát hiện tblUser
        [NotMapped]
        public virtual tblUser CustomerUser { get; set; }
        [NotMapped]
        public virtual tblUser AdminUser { get; set; }
        
        public virtual ICollection<tblChatConversationMessage> tblChatConversationMessages { get; set; }
    }
}
