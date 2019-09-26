using System;
using System.Data.Common;
using System.Threading.Tasks;
using CommandLine;
using MonitoringBot.Util;
using MySql.Data.MySqlClient;

namespace MonitoringBot.MySQL
{
    class MySQLClient
    {
        private MySqlConnection connection;

        public MySQLClient(Options options)
        {

            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder()
            {
                Server = options.Server,
                UserID = options.User,
                Password = options.Password,
                Database = "messages",
                Port = 3306
            };

            connection = new MySqlConnection(sb.ToString());
            Debug.Log("Connection Open");
            connection.Open();
        }

        ~MySQLClient()
        {
            Debug.Log("Connection Close");
            connection.Clone();
        }

        // time, user, id, is_update, text
        public async Task InsertDirectMessage(ulong user, ulong id, bool isUpdate, string text, bool isBot)
        {
            MySqlTransaction tx = null;
            try
            {
                using (var command = connection.CreateCommand())
                {
                    tx = connection.BeginTransaction();
                    command.CommandText =
                        "INSERT INTO direct_messages(user, id, is_update, text, is_bot) VALUES(@user, @id, @is_update, @text, @is_bot)";
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@is_update", isUpdate);
                    command.Parameters.AddWithValue("@text", text ?? "");
                    command.Parameters.AddWithValue("@is_bot", isBot);
                    Debug.Log($"ExecuteNonQueryAsync");
                    await command.ExecuteNonQueryAsync();
                    Debug.Log("success");
                    MySqlCommandBuilder b = new MySqlCommandBuilder()
                    {
                    };
                }
                tx.Commit();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                tx.Rollback();
            }
        }

        public async Task InsertGuildMessage(ulong guild, ulong channel, ulong user, ulong id,
            bool isUpdate, string text, bool isBot)
        {
            MySqlTransaction tx = null;
            try
            {
                using (var command = connection.CreateCommand())
                {
                    tx = connection.BeginTransaction();
                    command.CommandText =
                        "INSERT INTO guild_messages(guild, channel, user, id, is_update, text, is_bot) VALUES(@guild, @channel, @user, @id, @is_update, @text, @is_bot)";
                    command.Parameters.AddWithValue("@guild", guild);
                    command.Parameters.AddWithValue("@channel", channel);
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@is_update", isUpdate);
                    command.Parameters.AddWithValue("@text", text ?? "");
                    command.Parameters.AddWithValue("@is_bot", isBot);
                    Debug.Log($"ExecuteNonQueryAsync");
                    await command.ExecuteNonQueryAsync();
                    Debug.Log("success");
                }
                tx.Commit();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                tx.Rollback();
            }
        }
    }
}
