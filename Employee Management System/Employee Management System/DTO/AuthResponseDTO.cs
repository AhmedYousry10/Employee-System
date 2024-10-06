namespace Employee_Management_System.DTO
{
    public class AuthResponseDTO
    {
        public string Message { get; set; }
        public Guid userId { get; set; }
        public string Token { get; set; }
        public string[] Role { get; set; }
    }
}
