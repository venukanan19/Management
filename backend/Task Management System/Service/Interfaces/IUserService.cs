using Task_Management_System.DTOs;

namespace Task_Management_System.Services.Interfaces
{
    public interface IUserService
    {
        int AddUser(CreateUserDto dto);

        List<UserWithTasksDto> GetAllUsers();

        UserWithTasksDto? GetUserById(int id);

        UserWithTasksDto? GetUserWithTasks(int id);   // ← புதுசா சேர்த்தது

        int DeleteUser(int id);
    }
}