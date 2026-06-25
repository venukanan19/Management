using Microsoft.AspNetCore.Mvc;
using Task_Management_System.DTOs;
using Task_Management_System.Services.Interfaces;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("add")]
        public IActionResult AddTask(CreateTaskItemDto dto)
        {
            try
            {
                var result = _taskService.AddTask(dto);
                return Ok(new { success = true, data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskService.GetAllTasks();
            return Ok(new { success = true, data = tasks });
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
                return NotFound(new { success = false, message = "Task not found" });

            return Ok(new { success = true, data = task });
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateTask(int id, UpdateTaskItemDto dto)
        {
            var result = _taskService.UpdateTask(id, dto);
            return Ok(new { success = true, data = result });
        }

        [HttpPut("changestatus/{id}")]
        public IActionResult ChangeStatus(int id, ChangeStatusDto dto)
        {
            var result = _taskService.ChangeStatus(id, dto);
            return Ok(new { success = true, data = result });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var result = _taskService.DeleteTask(id);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("search")]
        public IActionResult SearchTasks(string keyword)
        {
            var tasks = _taskService.SearchTasks(keyword);
            return Ok(new { success = true, data = tasks });
        }
    }
}
