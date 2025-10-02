using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using giadinhthoxinh.Models;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            var messages = new List<string>(); // Thay đổi thành List<string> để hiển thị

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("SELECT UserName, Message, SentAt FROM tblChatMessage ORDER BY SentAt ASC", conn))
                {
                    conn.Open();
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var userName = rd["UserName"].ToString();
                        var message = rd["Message"].ToString();
                        var sentAt = Convert.ToDateTime(rd["SentAt"]);

                        // Format tin nhắn để hiển thị
                        var formattedMessage = $"<div class='{(userName == "Admin" ? "text-primary" : "text-success")}'><b>{userName}:</b> {message} <small>({sentAt:HH:mm})</small></div>";
                        messages.Add(formattedMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine("Load messages error: " + ex.Message);
                messages.Add("<div class='text-danger'>Không thể tải tin nhắn cũ</div>");
            }

            ViewBag.Messages = messages;
            return View();
        }
    }
}