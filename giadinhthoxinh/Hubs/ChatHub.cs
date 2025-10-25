using Microsoft.AspNet.SignalR;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Linq;

namespace giadinhthoxinh.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<int, string> conversationConnections = new ConcurrentDictionary<int, string>();
        private static ConcurrentDictionary<string, string> customerConnections = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> adminConnections = new ConcurrentDictionary<string, string>();

        public void JoinConversation(int conversationId, string customerEmail, string customerName)
        {
            try
            {
                Debug.WriteLine($"[ChatHub] Customer joining conversation: {conversationId}, Email: {customerEmail}, Name: {customerName}");
                conversationConnections[conversationId] = Context.ConnectionId;
                customerConnections[customerEmail] = Context.ConnectionId;
                
                Debug.WriteLine($"[ChatHub] Sending notification to AdminGroup...");
                Clients.Group("AdminGroup").customerConnected(conversationId, customerEmail, customerName);
                
                Debug.WriteLine($"[ChatHub] Customer joined successfully: {Context.ConnectionId}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ChatHub] ERROR JoinConversation: {ex.Message}");
                Debug.WriteLine($"[ChatHub] Stack trace: {ex.StackTrace}");
            }
        }

        public void JoinAsAdmin(string adminName)
        {
            try
            {
                Debug.WriteLine($"[ChatHub] Admin joining: {adminName}, ConnectionId: {Context.ConnectionId}");
                Groups.Add(Context.ConnectionId, "AdminGroup");
                adminConnections[Context.ConnectionId] = adminName;
                Debug.WriteLine($"[ChatHub] Admin joined AdminGroup successfully!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ChatHub] ERROR JoinAsAdmin: {ex.Message}");
                Debug.WriteLine($"[ChatHub] Stack trace: {ex.StackTrace}");
            }
        }

        public void PickupConversation(int conversationId, int adminAccountId, string adminName)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE tblChatConversation SET FK_iAdminAccountID = @adminId, sAdminName = @adminName, sStatus = N'Đang chat', dUpdatedAt = GETDATE() WHERE PK_iConversationID = @convId";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@adminId", adminAccountId);
                        cmd.Parameters.AddWithValue("@adminName", adminName);
                        cmd.Parameters.AddWithValue("@convId", conversationId);
                        cmd.ExecuteNonQuery();
                    }
                }
                if (conversationConnections.TryGetValue(conversationId, out string customerConnectionId))
                {
                    Clients.Client(customerConnectionId).adminJoined(adminName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR PickupConversation: {ex.Message}");
            }
        }

        public void SendConversationMessage(int conversationId, string senderType, string senderName, string message, int? senderAccountId = null)
        {
            try
            {
                int messageId = SaveConversationMessage(conversationId, senderType, senderName, message, senderAccountId);
                if (messageId > 0)
                {
                    var messageData = new { messageId = messageId, conversationId = conversationId, senderType = senderType, senderName = senderName, message = message, sentAt = DateTime.Now.ToString("HH:mm") };
                    if (conversationConnections.TryGetValue(conversationId, out string customerConnectionId))
                    {
                        Clients.Client(customerConnectionId).receiveMessage(messageData);
                    }
                    Clients.Group("AdminGroup").receiveMessage(messageData);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.receiveError("Không thể gửi tin nhắn!");
            }
        }

        private int SaveConversationMessage(int conversationId, string senderType, string senderName, string message, int? senderAccountId)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO tblChatConversationMessage (FK_iConversationID, senderType, FK_iSenderAccountID, sSenderName, sMessage, dSentAt, bIsRead) VALUES (@convId, @type, @senderId, @name, @msg, GETDATE(), 0); SELECT CAST(SCOPE_IDENTITY() as int);";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@convId", conversationId);
                        cmd.Parameters.AddWithValue("@type", senderType);
                        cmd.Parameters.AddWithValue("@senderId", (object)senderAccountId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@name", senderName);
                        cmd.Parameters.AddWithValue("@msg", message);
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void SendMessage(string user, string message)
        {
            try
            {
                bool saveSuccess = SaveMessage(user, message);
                if (saveSuccess)
                {
                    Clients.All.broadcastMessage(user, message);
                }
                else
                {
                    Clients.Caller.broadcastMessage("System", "Lỗi: Không thể lưu tin nhắn!");
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.broadcastMessage("System", "Đã xảy ra lỗi khi gửi tin nhắn!");
            }
        }

        private bool SaveMessage(string user, string message)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                if (string.IsNullOrEmpty(connStr)) return false;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "INSERT INTO tblChatMessage (UserName, Message, SentAt) VALUES (@u, @m, @t)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", user);
                        cmd.Parameters.AddWithValue("@m", message);
                        cmd.Parameters.AddWithValue("@t", DateTime.Now);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var convToRemove = conversationConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (convToRemove.Key != 0)
            {
                conversationConnections.TryRemove(convToRemove.Key, out _);
            }
            var custToRemove = customerConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (custToRemove.Key != null)
            {
                customerConnections.TryRemove(custToRemove.Key, out _);
            }
            adminConnections.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnected(stopCalled);
        }
    }
}
