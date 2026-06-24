namespace Task_Management_System.DTOs
{
    public class TaskItemResponseDto
    {
        public int TaskId { get; set; }
        public string ? Title { get; set; }
        public string ? Description { get; set; }
        public string ? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public String ? UserName { get; set; }
    }
}
