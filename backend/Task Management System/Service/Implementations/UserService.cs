using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories.Interfaces;
using Task_Management_System.Services.Interfaces;

namespace Task_Management_System.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

   
        public int AddUser(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
                throw new ArgumentException("User name cannot be empty");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email cannot be empty");


            return _userRepository.AddUser(dto);
        }

        public List<UserWithTasksDto> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return users.Select(u => new UserWithTasksDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Tasks = new List<TaskItemResponseDto>() 
            }).ToList();
        }


        public UserWithTasksDto? GetUserById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user ID");

            var user = _userRepository.GetUserById(id);
            if (user == null) return null;

            return new UserWithTasksDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Tasks = new List<TaskItemResponseDto>()
            };
        }


        public int DeleteUser(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user ID");

            return _userRepository.DeleteUser(id);
        }
    }
}
