using System.Collections.Generic;
using Task_Management_System.Models;
using Task_Management_System.DTOs;

namespace Task_Management_System.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<UserWithTasksDto> GetAllUsers();
        UserWithTasksDto? GetUserById(int id);
        int AddUser(CreateUserDto dto);
        UserWithTasksDto GetUserWithTasks(int id);
        int DeleteUser(int id);
    }
}
