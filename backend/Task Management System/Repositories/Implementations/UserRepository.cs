using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories.Interfaces;



namespace Task_Management_System.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
                
        }

        public int AddUser(CreateUserDto dto)
        {
            string Connectionstring = _connectionString.GetConnectionString("DefaultConnection")!;
            using var connection = new SqlConnection(Connectionstring);
            connection.Open();

            var sql = @"INSERT INTO Users (UserName, Email)
                        VALUES (@UserName, @Email);
                        SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@UserName", dto.UserName);
            cmd.Parameters.AddWithValue("@Email", dto.Email);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<UserWithTasksDto> GetAllUsers()
        {
            string Connectionstring = _connectionString.GetConnectionString("DefaultConnection")!;

            var users = new List<UserWithTasksDto>();
            using var connection = new SqlConnection(Connectionstring);
            connection.Open();

            var sql = "SELECT UserId, UserName, Email FROM Users";
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new UserWithTasksDto
                {
                    UserId = (int)reader["UserId"],
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Tasks = new List<TaskItemResponseDto>()
                });
            }

            return users;
        }


        public UserWithTasksDto? GetUserById(int id)
        {
            string Connectionstring = _connectionString.GetConnectionString("DefaultConnection")!;

            using var connection = new SqlConnection(Connectionstring);
            connection.Open();

            var sql = @"SELECT UserId, UserName, Email FROM Users WHERE UserId=@UserId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@UserId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserWithTasksDto
                {
                    UserId = (int)reader["UserId"],
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString()
                };
            }

            return null;
        }

        public UserWithTasksDto GetUserWithTasks(int id)
        {
            string Connectionstring = _connectionString.GetConnectionString("DefaultConnection")!;

            var userWithTasks = new UserWithTasksDto
            {
                Tasks = new List<TaskItemResponseDto>()
            };

            using var connection = new SqlConnection(Connectionstring);
            connection.Open();

            var sql = @"SELECT u.UserId, u.Username, u.Email,
                        t.TaskId, t.Title, t.Description, t.Status FROM Users u
                        LEFT JOIN Tasks t ON u.UserId = t.UserId
                        WHERE u.UserId = @UserId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@UserId", id);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (userWithTasks.UserId == 0)
                {
                    userWithTasks.UserId = (int)reader["UserId"];
                    userWithTasks.UserName = reader["UserName"].ToString();
                    userWithTasks.Email = reader["Email"].ToString();
                }

                if (reader["TaskId"] != DBNull.Value)
                {
                    userWithTasks.Tasks.Add(new TaskItemResponseDto
                    {
                        TaskId = (int)reader["TaskId"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString(),
                        UserId = id
                    });
                }
            }

            return userWithTasks;
        }
        public int DeleteUser(int id)
{
            string Connectionstring = _connectionString.GetConnectionString("DefaultConnection")!;

            using var connection = new SqlConnection(Connectionstring);
    connection.Open();
    var sql = "DELETE FROM Users WHERE UserId = @Id";
    using var cmd = new SqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@Id", id);
    return cmd.ExecuteNonQuery();
}

    }
}

    
        