using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories.Implementations
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public List<TaskItemResponseDto> GetAllTasks()
        {
            var tasks = new List<TaskItemResponseDto>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = "SELECT TaskId, Title, Description, Status, UserId FROM Tasks";
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
                    UserId = (int)reader["UserId"]
                });
            }

            return tasks;
        }

        public TaskItemResponseDto GetTaskById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sql = @"SELECT TaskId, Title, Description, Status, UsreId FROM Tasks WHERE TaskId=@TaskId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@TaskId", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TaskItemResponseDto
                {
                    UserId = (int)reader["TaskId"],
                    TaskId = (int)reader["TaskId"],
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = reader["Status"].ToString()
                };
            }

            return null;
        }
    }
}
