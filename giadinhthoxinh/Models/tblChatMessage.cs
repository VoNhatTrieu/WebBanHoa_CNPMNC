using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giadinhthoxinh.Models
{
	public class tblChatMessage
	{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}