using System;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class UserRepository
    {
        public User GetByUsername(string username)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM users WHERE username = @username";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    Id = Convert.ToInt32(reader["user_id"]),
                                    Username = reader["username"].ToString(),
                                    PasswordHash = reader["password_hash"].ToString(),
                                    Role = reader["role"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error fetching user by username: {username}");
                throw;
            }
            return null;
        }

        public void Create(User user, string email)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO users (username, password_hash, email, role) VALUES (@username, @password_hash, @email, @role)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@role", user.Role);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error creating user: {user.Username}");
                throw;
            }
        }

        public bool UsernameExists(string username)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error checking username existence: {username}");
                throw;
            }
        }

        public bool EmailExists(string email)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error checking email existence: {email}");
                throw;
            }
        }
    }
}
