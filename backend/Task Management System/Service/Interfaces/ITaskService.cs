using Task_Management_System.DTOs;

namespace Task_Management_System.Services.Interfaces
{
    public interface ITaskService
    {
        int AddTask(CreateTaskItemDto dto);
        List<TaskItemResponseDto> GetAllTasks();
        TaskItemResponseDto? GetTaskById(int id);
        int UpdateTask(int id, UpdateTaskItemDto dto);
        int ChangeStatus(int id, ChangeStatusDto dto);
        int DeleteTask(int id);
        List<TaskItemResponseDto> SearchTasks(string keyword);
       
    }
}
