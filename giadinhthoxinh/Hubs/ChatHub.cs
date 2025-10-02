using Microsoft.AspNet.SignalR;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace giadinhthoxinh.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string user, string message)
        {
            try
            {
                Debug.WriteLine($"=== CHATHUB DEBUG ===");
                Debug.WriteLine($"Received message from: {user}");
                Debug.WriteLine($"Message content: {message}");
                Debug.WriteLine($"ConnectionId: {Context.ConnectionId}");
                Debug.WriteLine($"Timestamp: {DateTime.Now}");

                // Validate input
                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(message))
                {
                    Debug.WriteLine("ERROR: Empty user or message");
                    return;
                }

                // Lưu tin nhắn vào DB trước
                bool saveSuccess = SaveMessage(user, message);

                if (saveSuccess)
                {
                    Debug.WriteLine("Database save: SUCCESS");

                    // Phát realtime tới tất cả client sau khi lưu thành công
                    Clients.All.broadcastMessage(user, message);
                    Debug.WriteLine("Broadcast to all clients: SUCCESS");
                }
                else
                {
                    Debug.WriteLine("Database save: FAILED - Not broadcasting");

                    // Gửi lỗi cho client gửi tin nhắn
                    Clients.Caller.broadcastMessage("System", "Lỗi: Không thể lưu tin nhắn!");
                }

                Debug.WriteLine("=== END CHATHUB DEBUG ===");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CHATHUB EXCEPTION: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // Gửi lỗi cho client
                Clients.Caller.broadcastMessage("System", "Đã xảy ra lỗi khi gửi tin nhắn!");
            }
        }

        private bool SaveMessage(string user, string message)
        {
            try
            {
                Debug.WriteLine("=== DATABASE SAVE DEBUG ===");

                // Lấy chuỗi kết nối
                string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                Debug.WriteLine($"Connection string exists: {!string.IsNullOrEmpty(connStr)}");

                if (string.IsNullOrEmpty(connStr))
                {
                    Debug.WriteLine("ERROR: Connection string is null or empty");
                    return false;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "INSERT INTO tblChatMessage (UserName, Message, SentAt) VALUES (@u, @m, @t)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", user);
                        cmd.Parameters.AddWithValue("@m", message);
                        cmd.Parameters.AddWithValue("@t", DateTime.Now);

                        Debug.WriteLine($"SQL: {sql}");
                        Debug.WriteLine($"Params: @u='{user}', @m='{message}', @t='{DateTime.Now}'");

                        conn.Open();
                        Debug.WriteLine("Database connection opened");

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"Rows affected: {rowsAffected}");

                        bool success = rowsAffected > 0;
                        Debug.WriteLine($"Save result: {(success ? "SUCCESS" : "FAILED")}");
                        Debug.WriteLine("=== END DATABASE DEBUG ===");

                        return success;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"SQL ERROR: {sqlEx.Message}");
                Debug.WriteLine($"SQL Error Number: {sqlEx.Number}");
                Debug.WriteLine($"SQL State: {sqlEx.State}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SAVE MESSAGE ERROR: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        // Override để debug connection events
        public override System.Threading.Tasks.Task OnConnected()
        {
            Debug.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            Debug.WriteLine($"Client disconnected: {Context.ConnectionId}, StopCalled: {stopCalled}");
            return base.OnDisconnected(stopCalled);
        }
    }
}