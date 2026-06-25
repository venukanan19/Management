using Microsoft.AspNetCore.Mvc;
using Task_Management_System.DTOs;
using Task_Management_System.Services.Interfaces;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add")]
        public IActionResult AddUser(CreateUserDto dto)
        {
            var result = _userService.AddUser(dto);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(new { success = true, data = users });
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, data = user });
        }

        // ← புது endpoint
        [HttpGet("{id}/tasks")]
        public IActionResult GetUserWithTasks(int id)
        {
            var userWithTasks = _userService.GetUserWithTasks(id);
            if (userWithTasks == null)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, data = userWithTasks });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            return Ok(new { success = true, data = result });
        }
    }
}