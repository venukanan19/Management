using Task_Management_System.DTOs;
using Task_Management_System.Repositories.Interfaces;
using Task_Management_System.Services.Interfaces;

namespace Task_Management_System.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        // Allowed status values, matches DB CHECK constraint
        private static readonly string[] ValidStatuses = { "Todo", "In Progress", "Done" };

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public int AddTask(CreateTaskItemDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Task title cannot be empty");

            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status cannot be empty");

            var validStatuses = new List<string> { "Todo", "In Progress", "Done" };
            if (!validStatuses.Contains(dto.Status))
                throw new ArgumentException("Status must be Todo, In Progress, or Done.");

            return _taskRepository.AddTask(dto);
        }


        public List<TaskItemResponseDto> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public TaskItemResponseDto? GetTaskById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID");

            return _taskRepository.GetTaskById(id);
        }

        public int UpdateTask(int id, UpdateTaskItemDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID");

            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status cannot be empty");

            var validStatuses = new List<string> { "Todo", "In Progress", "Done" };
            if (!validStatuses.Contains(dto.Status))
                throw new ArgumentException("Status must be Todo, In Progress, or Done.");

            return _taskRepository.UpdateTask(id, dto);
        }


        public int ChangeStatus(int id, ChangeStatusDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID");

            if (string.IsNullOrWhiteSpace(dto.Status) || !ValidStatuses.Contains(dto.Status))
                throw new ArgumentException("Status must be Todo, In Progress, or Done.");

            return _taskRepository.ChangeStatus(id, dto);
        }

        public int DeleteTask(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID");

            return _taskRepository.DeleteTask(id);
        }

        public List<TaskItemResponseDto> SearchTasks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Search keyword cannot be empty");

            return _taskRepository.SearchTasks(keyword);
        }
    }
}