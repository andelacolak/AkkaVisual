using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Akka.NetVisualAPI.Helpers
{
    public static class DBHelper
    {
        private static string connectionString { get; } = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Andela\Desktop\Anđela\pmf\Raspodijeljeni sustavi\acolak_rs_projekt\MailboxLibrary\API\Akka.NetVisualAPI\Akka.NetVisualAPI\App_Data\VisualDB.mdf';Integrated Security=True";
        
        public static void AddConnection(string userId, string userConnection)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"insert into [dbo].[Client] ([id], [UserId]) values(@userConnection, @userId)";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@userConnection", userConnection);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RemoveConnection(string userConnection)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"DELETE FROM [dbo].[Client] WHERE Id='@connection';";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@connection", userConnection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<string> GetConnectionIds(string user)
        {
            List<string> connections = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"SELECT Id FROM [dbo].[Client] WHERE [UserId]='?';".Replace("?", user);
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             connections.Add(reader["Id"].ToString());
                        }
                    }
                }
            }

            return connections;
        }
    }
}