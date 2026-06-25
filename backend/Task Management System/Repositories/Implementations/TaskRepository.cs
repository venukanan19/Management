using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories.Interfaces;

namespace Task_Management_System.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;
        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }

        public List<TaskItemResponseDto> GetAllTasks()
        {
            var tasks = new List<TaskItemResponseDto>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"SELECT t.TaskId, t.Title, t.Description, t.Status, t.CreatedDate,
                       t.UserId, u.UserName
                FROM Tasks t
                INNER JOIN Users u ON t.UserId = u.UserId";

            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new TaskItemResponseDto
                {
                    TaskId = (int)reader["TaskId"],
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = reader["Status"].ToString(),
                    CreatedDate = (DateTime)reader["CreatedDate"],
                    UserId = (int)reader["UserId"],
                    UserName = reader["UserName"].ToString()
                });
            }

            return tasks;
        }

        public TaskItemResponseDto? GetTaskById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"SELECT t.TaskId, t.Title, t.Description, t.Status, t.CreatedDate,
                       t.UserId, u.UserName
                FROM Tasks t
                INNER JOIN Users u ON t.UserId = u.UserId
                WHERE t.TaskId = @TaskId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@TaskId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TaskItemResponseDto
                {
                    TaskId = (int)reader["TaskId"],
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = reader["Status"].ToString(),
                    CreatedDate = (DateTime)reader["CreatedDate"],
                    UserId = (int)reader["UserId"],
                    UserName = reader["UserName"].ToString()
                };
            }

            return null;
        }

        public int AddTask(CreateTaskItemDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"INSERT INTO Tasks (Title, Description, Status, UserId)
                        VALUES (@Title, @Description, @Status, @UserId);
                        SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@Status", dto.Status);
            cmd.Parameters.AddWithValue("@UserId", dto.UserId);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int UpdateTask(int id, UpdateTaskItemDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"UPDATE Tasks
                SET Title = @Title,
                    Description = @Description,
                    Status = @Status
                    UserId=@UserId,
                WHERE TaskId = @TaskId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@Status", dto.Status);
            cmd.Parameters.AddWithValue("@UserId", dto.UserId);
            cmd.Parameters.AddWithValue("@TaskId", id);

            return cmd.ExecuteNonQuery();
        }

        public int DeleteTask(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"DELETE FROM Tasks WHERE TaskId = @TaskId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@TaskId", id);

            return cmd.ExecuteNonQuery();
        }

        public List<TaskItemResponseDto> SearchTasks(string keyword)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"SELECT TaskId, Title, Description, Status, UserId
                FROM Tasks
                WHERE Title LIKE @Keyword OR Description LIKE @Keyword";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

            using var reader = cmd.ExecuteReader();
            var tasks = new List<TaskItemResponseDto>();

            while (reader.Read())
            {
                tasks.Add(new TaskItemResponseDto
                {
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = reader["Status"].ToString(),
                    UserId = Convert.ToInt32(reader["UserId"])
                });
            }

            return tasks;
        }

        public int ChangeStatus(int id, ChangeStatusDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"UPDATE Tasks SET Status = @Status WHERE TaskId = @TaskId";
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Status", dto.Status);
            cmd.Parameters.AddWithValue("@TaskId", id);

            return cmd.ExecuteNonQuery();
        }



    }
}
