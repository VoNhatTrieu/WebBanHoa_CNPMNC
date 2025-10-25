//using giadinhthoxinh.Models;
//using System;
//using System.Linq;
//using System.Web.Mvc;

//namespace giadinhthoxinh.Areas.Admin.Controllers
//{
//    public class ChatController : Controller
//    {
//        private ChatDbContext db = new ChatDbContext();

//        public ActionResult Index()
//        {
//            var admin = Session["User"] as tblUser;
            
//            // Nếu admin đã đăng nhập, dùng thông tin admin
//            if (admin != null)
//            {
//                ViewBag.AdminName = admin.sUserName;
//                ViewBag.AdminAccountId = admin.PK_iAccountID;
//            }
//            else
//            {
//                // Nếu chưa đăng nhập, dùng giá trị mặc định
//                ViewBag.AdminName = "Admin";
//                ViewBag.AdminAccountId = 0;
//            }
            
//            return View();
//        }

//        [HttpGet]
//        public JsonResult GetPendingConversations()
//        {
//            try
//            {
//                var conversations = db.tblChatConversations
//                    .Where(c => c.sStatus == "Chờ admin" || c.sStatus == "Đang chat")
//                    .OrderByDescending(c => c.dUpdatedAt)
//                    .Select(c => new
//                    {
//                        conversationId = c.PK_iConversationID,
//                        customerEmail = c.sCustomerEmail,
//                        customerName = c.sCustomerName,
//                        status = c.sStatus,
//                        adminName = c.sAdminName,
//                        lastMessage = db.tblChatConversationMessages
//                            .Where(m => m.FK_iConversationID == c.PK_iConversationID)
//                            .OrderByDescending(m => m.dSentAt)
//                            .Select(m => m.sMessage)
//                            .FirstOrDefault(),
//                        updatedAt = c.dUpdatedAt
//                    })
//                    .ToList();

//                return Json(new { success = true, data = conversations }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpGet]
//        public JsonResult GetConversationMessages(int conversationId)
//        {
//            try
//            {
//                var messages = db.tblChatConversationMessages
//                    .Where(m => m.FK_iConversationID == conversationId)
//                    .OrderBy(m => m.dSentAt)
//                    .Select(m => new
//                    {
//                        messageId = m.PK_iMessageID,
//                        senderType = m.senderType,
//                        senderName = m.sSenderName,
//                        message = m.sMessage,
//                        sentAt = m.dSentAt
//                    })
//                    .ToList();

//                return Json(new { success = true, data = messages }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public JsonResult CreateConversation(string customerEmail, string customerName, int? customerAccountId)
//        {
//            try
//            {
//                var existingConversation = db.tblChatConversations
//                    .FirstOrDefault(c => c.sCustomerEmail == customerEmail && 
//                                       (c.sStatus == "Chờ admin" || c.sStatus == "Đang chat"));

//                if (existingConversation != null)
//                {
//                    return Json(new { 
//                        success = true, 
//                        conversationId = existingConversation.PK_iConversationID,
//                        message = "Đã kết nối lại cuộc trò chuyện" 
//                    }, JsonRequestBehavior.AllowGet);
//                }

//                var conversation = new tblChatConversation
//                {
//                    FK_iCustomerAccountID = customerAccountId,
//                    sCustomerEmail = customerEmail,
//                    sCustomerName = customerName,
//                    sStatus = "Chờ admin",
//                    dCreatedAt = DateTime.Now,
//                    dUpdatedAt = DateTime.Now
//                };

//                db.tblChatConversations.Add(conversation);
//                db.SaveChanges();

//                return Json(new { 
//                    success = true, 
//                    conversationId = conversation.PK_iConversationID,
//                    message = "Đã tạo cuộc trò chuyện mới" 
//                }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
