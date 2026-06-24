using Task_Management_System.DTOs;

namespace Task_Management_System.Services.Interfaces
{
    public interface IUserService
    {
        int AddUser(CreateUserDto dto);

        List<UserWithTasksDto> GetAllUsers();

        UserWithTasksDto? GetUserById(int id);

        int DeleteUser(int id);
    }
}
