using System.Collections.Generic;
using Task_Management_System.DTOs;


namespace Task_Management_System.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        List<TaskItemResponseDto> GetAllTasks();
        TaskItemResponseDto GetTaskById(int id);
        int AddTask(CreateTaskItemDto dto);
        int UpdateTask(int id, UpdateTaskItemDto dto);
        int ChangeStatus(int id, ChangeStatusDto dto);
        int DeleteTask(int id);

        List<TaskItemResponseDto> SearchTasks(String name);
    }
}
