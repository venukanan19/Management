namespace Task_Management_System.DTOs
{
    public class UserWithTasksDto
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public List<TaskItemResponseDto> Tasks { get; set; }
    }
}
